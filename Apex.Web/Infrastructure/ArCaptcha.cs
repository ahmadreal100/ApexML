using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Apex.Service.Translations;
using Apex.Shared.Extensions;

namespace Apex.Web.Infrastructure
{
    public class ArCaptchaAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userAnswer = filterContext.RequestContext.HttpContext.Request.Form["ArCaptcha"];

            if (userAnswer.IsNeu())
                filterContext.Controller.ViewData.ModelState.AddModelError("ArCaptcha", Str.rq.Ft(Str.securityPicture));
            else
            {
                var rightAnswer = ArCaptcha.Answer.ToLower();
                if (rightAnswer != userAnswer)
                    filterContext.Controller.ViewData.ModelState.AddModelError("ArCaptcha", Str.isIncorrect.Ft(Str.securityPicture));
            }

            base.OnActionExecuting(filterContext);
        }
    }

    public static class ArCaptchaHtmlHelper
    {
        public static MvcHtmlString ArCaptcha(this HtmlHelper helper, string name, object htmlAttributes)
        {
            var txt = new TagBuilder("input");
            txt.MergeAttribute("name", name);
            txt.MergeAttribute("id", name.Replace(".", "_"));
            if (htmlAttributes != null)
                txt.MergeAttributes(htmlAttributes.ToDictionary(), true);
            return MvcHtmlString.Create(txt.ToString());
        }

        public static MvcHtmlString ArCaptcha(this HtmlHelper helper, object htmlAttributes)
        {
            return helper.ArCaptcha("ArCaptcha", htmlAttributes);
        }

        public static MvcHtmlString ArCaptcha(this HtmlHelper helper)
        {
            return helper.ArCaptcha("ArCaptcha", null);
        }

        public static MvcHtmlString ArCaptcha(this HtmlHelper helper, string name)
        {
            //s.
            return helper.ArCaptcha(name, null);
        }
    }

    #region ArCaptchaImage

    public class ArCaptchaSetting
    {
        public ArCaptchaType Type { get; set; } = ArCaptchaType.DigitLetter;
        public int Width { get; set; } = 100;
        public int Height { get; set; } = 50;

        public Font Font { get; set; } = new Font(new FontFamily("Tahoma"), 13f, FontStyle.Bold);
        public int MinNumber { get; set; } = 0;
        public int MaxNumber { get; set; } = 50;

        public byte StringLength { get; set; } = 5;
        public StringFormatFlags StringFormatFlags { get; set; }

        public Color FontColor { get; set; } = Color.Black;
        public Color BackColor { get; set; } = Color.White;

        public bool RandomFontColor { get; set; } = false;

        public byte MinRangeColor { get; set; } = 150;
        public byte MaxRangeColor { get; set; } = 200;

        public int MinRangeRotate { get; set; } = -10;
        public int MaxRangeRotate { get; set; } = 10;

        public bool FillShape { get; set; } = false;
        public int ShapeWidth { get; set; } = 2;

        public int NoiseNumber { get; set; } = 50;
        public int NoiseCrossBox { get; set; } = 5;

        public ArCaptchaNoiseType NoiseType { get; set; } = ArCaptchaNoiseType.Circle;
    }

    public enum ArCaptchaNoiseType
    {
        Pixel = 0,
        Circle = 1,
        Rectangle = 2,
        Line = 3
    }

    public enum ArCaptchaLanguage
    {
        Persian = 0,
        English = 1
    }

    public enum ArCaptchaType
    {
        Digit = 0,
        Letter = 1,
        DigitLetter = 2,
        Mathematical = 3,
        PersianLetter = 4,
        EnglishLetter = 5
    }

    public class ArCaptcha
    {
        #region Private Fields

        private const string SessionKey = "Ss_ArCaptcha";

        private const int AsciA = 65;
        private const int AsciZ = 90;

        private const int Asci0 = 48;
        private const int Asci9 = 57;

        private const int AsciPlus = 43;
        private const int AsciMinus = 45;

        private readonly ArCaptchaSetting _option;

        private int _convertedLetterAnswer;
        private readonly Random _bigRandom = new Random(DateTime.Now.Millisecond * DateTime.Now.Minute * DateTime.Now.Hour);

        private int Random() => Random(_option.MinNumber, _option.MaxNumber + 1);
        private int Random(int min, int max) => _bigRandom.Next(min, max);

        private int RandomSwitch(int a, int b) => Random(1, 3) == 1 ? a : b;
        private int RandomNoise => Random(_option.MinRangeColor, _option.MaxRangeColor + 1);
        private Color RandomColor() => Color.FromArgb(byte.MaxValue, RandomNoise, RandomNoise, RandomNoise);
        private char RandomLetter => (char)Random(AsciA, AsciZ + 1);
        private char RandomDigitLetter => (char)RandomSwitch(Random(AsciA, AsciZ + 1), Random(Asci0, Asci9 + 1));
        private char RandomOperator => (char)RandomSwitch(AsciPlus, AsciMinus);

        #endregion

        #region Constructors

        public ArCaptcha()
        {
            _option = new ArCaptchaSetting();
        }

        public ArCaptcha(ArCaptchaSetting option)
        {
            _option = option ?? new ArCaptchaSetting();
        }

        #endregion

        public Image Generate(out string answer)
        {
            var bitmap = new Bitmap(_option.Width, _option.Height);
            var graphics = Graphics.FromImage(bitmap);

            var captcha = GetCaptchaString();
            answer = GetAnswer(captcha);
            SetAnswer(answer);

            graphics.FillRectangle(new SolidBrush(_option.BackColor),
                new Rectangle(0, 0, _option.Width, _option.Height));

            SetNoise(ref graphics);

            var sizeF = graphics.MeasureString(captcha, _option.Font, new SizeF(_option.Width - 2, _option.Height - 2));
            var format = new StringFormat(_option.StringFormatFlags) { Alignment = StringAlignment.Center };

            var rect = new RectangleF(_option.Width / 2f - sizeF.Width / 2f, _option.Height / 2f - sizeF.Height / 2f,
                sizeF.Width, sizeF.Width);
            var cen = new Point((int)(rect.Left + rect.Width / 2), (int)(rect.Top + rect.Height / 2));

            graphics.TranslateTransform(cen.X, cen.Y);
            graphics.RotateTransform(Random(_option.MinRangeRotate, _option.MaxRangeRotate));
            graphics.TranslateTransform(-cen.X, -cen.Y);

            var fc = _option.RandomFontColor ? RandomColor() : _option.FontColor;
            graphics.DrawString(captcha, _option.Font, new SolidBrush(fc), rect, format);
            graphics.ResetTransform();
            return bitmap;
        }

        private void SetAnswer(string answer)
        {
            HttpContext.Current.Session[SessionKey] = answer;
        }

        public static string Answer => HttpContext.Current.Session[SessionKey].ToString();

        public string ConvertToPersianLetter(double number) =>
            new ArCaptchaNumberToLetter(ArCaptchaLanguage.Persian).Convert(number);

        public string ConvertToEnglishLetter(double number) =>
            new ArCaptchaNumberToLetter(ArCaptchaLanguage.English).Convert(number);

        private string GetCaptchaString()
        {
            switch (_option.Type)
            {
                case ArCaptchaType.Digit:
                    return Random().ToString();

                case ArCaptchaType.Letter:
                    var l = string.Empty;
                    for (var i = 0; i < _option.StringLength; i++)
                        l += RandomLetter;
                    return l;

                case ArCaptchaType.DigitLetter:
                    var ld = string.Empty;
                    for (var i = 0; i < _option.StringLength; i++)
                        ld += RandomDigitLetter;
                    return ld;

                case ArCaptchaType.Mathematical:
                    var a = Random();
                    var b = Random();
                    var max = Math.Max(a, b);
                    var min = Math.Max(a, b);
                    var op = RandomOperator;
                    return op == (char)AsciMinus ? $"{max} {op} {min}" : $"{a} {op} {b}";

                case ArCaptchaType.PersianLetter:
                    _convertedLetterAnswer = Random();
                    return ConvertToPersianLetter(_convertedLetterAnswer);

                case ArCaptchaType.EnglishLetter:
                    _convertedLetterAnswer = Random();
                    return ConvertToEnglishLetter(_convertedLetterAnswer);

                default:
                    return null;
            }
        }

        private string GetAnswer(string captcha)
        {
            captcha = captcha.Replace(" ", "");
            if (_option.Type == ArCaptchaType.PersianLetter)
                return _convertedLetterAnswer.ToString();

            return Regex.IsMatch(captcha, @"\d+[+-]\d+") ? new DataTable().Compute(captcha, null).ToString() : captcha;
        }

        private void SetNoise(ref Graphics graphics)
        {
            switch (_option.NoiseType)
            {
                case ArCaptchaNoiseType.Pixel:
                    for (var index = 0; index < _option.NoiseNumber; ++index)
                        graphics.DrawEllipse(new Pen(RandomColor(), 1), Random(0, _option.Width),
                            Random(0, _option.Height), 1, 1);
                    break;
                case ArCaptchaNoiseType.Circle:
                    if (_option.FillShape)
                        for (var index = 0; index < _option.NoiseNumber; ++index)
                        {
                            var w = Random(5, 15);
                            graphics.FillEllipse(new SolidBrush(RandomColor()),
                                Random(0 - _option.NoiseCrossBox, _option.Width + _option.NoiseCrossBox),
                                Random(0 - _option.NoiseCrossBox, _option.Height + _option.NoiseCrossBox), w, w);
                        }
                    else
                        for (var index = 0; index < _option.NoiseNumber; ++index)
                        {
                            var w = Random(5, 15);
                            graphics.DrawEllipse(new Pen(RandomColor(), _option.ShapeWidth),
                                Random(0 - _option.NoiseCrossBox, _option.Width + _option.NoiseCrossBox),
                                Random(0 - _option.NoiseCrossBox, _option.Height + _option.NoiseCrossBox), w, w);
                        }

                    break;
                case ArCaptchaNoiseType.Rectangle:

                    if (_option.FillShape)
                        for (var index = 0; index < _option.NoiseNumber; ++index)
                        {
                            var w = Random(5, 15);
                            graphics.FillRectangle(new SolidBrush(RandomColor()),
                                Random(0 - _option.NoiseCrossBox, _option.Width + _option.NoiseCrossBox),
                                Random(0 - _option.NoiseCrossBox, _option.Height + _option.NoiseCrossBox), w, w);
                        }
                    else
                        for (var index = 0; index < _option.NoiseNumber; ++index)
                        {
                            var w = Random(5, 15);
                            graphics.DrawRectangle(new Pen(RandomColor(), _option.ShapeWidth),
                                Random(0 - _option.NoiseCrossBox, _option.Width + _option.NoiseCrossBox),
                                Random(0 - _option.NoiseCrossBox, _option.Height + _option.NoiseCrossBox), w, w);
                        }

                    break;
                case ArCaptchaNoiseType.Line:

                    for (var index = 0; index < _option.NoiseNumber; ++index)
                        graphics.DrawLine(new Pen(RandomColor(), 1), Random(0, _option.Width),
                            Random(0, _option.Height), Random(0, _option.Width), Random(0, _option.Height));
                    break;
            }
        }

        private class ArCaptchaNumberToLetter
        {
            private static string _and;
            private static string _zero;
            private static string _negative;

            private static string[] _ones;
            private static string[] _tenToTwenty;
            private static string[] _tens;
            private static string[] _hundreds;
            private static string[] _thousands;
            private static string[] _decimalPlace;

            public ArCaptchaNumberToLetter(ArCaptchaLanguage lang)
            {
                switch (lang)
                {
                    case ArCaptchaLanguage.Persian:
                        _and = " و ";
                        _zero = "صفر";
                        _negative = "منفی";

                        _ones = OnesFa;
                        _tenToTwenty = TenToTwentyFa;
                        _tens = TensFa;
                        _hundreds = HundredsFa;
                        _thousands = ThousandsFa;
                        _decimalPlace = DecimalFa;
                        break;
                    case ArCaptchaLanguage.English:
                        _and = " ";
                        _zero = "zero";
                        _negative = "negative";

                        _ones = OnesEn;
                        _tenToTwenty = TenToTwentyEn;
                        _tens = TensEn;
                        _hundreds = HundredsEn;
                        _thousands = ThousandsEn;
                        _decimalPlace = DecimalEn;
                        break;
                }
            }

            /// <summary>
            /// Convert number to persian string.
            /// </summary>
            /// <param name="number">number to convert.</param>
            /// <returns></returns>
            public string Convert(double number)
            {
                var digit = number.ToString("0." + new string('#', 21));
                if (double.Parse(digit).Equals(0))
                    return _zero;
                var isNegative = digit.Contains("-");
                var hasDecimal = digit.Contains(".");
                var entireArray = Regex.Split(Regex.Replace(digit, "-", ""), "\\D");
                var round = entireArray[0];

                var ret = CycleToConvert(round);
                var n = "";
                if (hasDecimal)
                    n = Regex.Replace(double.Parse(entireArray[1].Insert(0, ".")).ToString("0." + new string('#', 21)),
                        "\\d+\\.", string.Empty);
                return
                    $"{(isNegative ? _negative + " " : "")}{ret.Trim()}{(hasDecimal ? $"{(ret.Trim() == _zero || string.IsNullOrEmpty(ret) ? "" : _and)}{BuildDecimalPlaceString(n).Trim()}" : "")}";
            }

            private static string BuildDecimalPlaceString(string num)
            {
                var ret = CycleToConvert(num);
                return ret != _zero ? $"{ret.Trim()} {_decimalPlace[num.Length - 1]}" : "";
            }

            private static string CycleToConvert(string num)
            {
                var ret = "";
                var mainStr = STR_To_Int(num);
                var q = 0;
                for (var i = mainStr.Length - 1; i >= 0; i--)
                {
                    var strva = string.IsNullOrWhiteSpace(ret) ? " " : _and;
                    var ns = Convert_STR(GetCountStr(mainStr[i]), q);
                    ret = (double.Parse(mainStr[i]) > 0 ? ns + strva : "") + ret;
                    q++;
                }

                if (ret == " " || ret == "  ")
                    ret = _zero;
                return ret;
            }

            private static string[] STR_To_Int(string str)
            {
                str = GetCountStr(str);
                var ret = new string[str.Length / 3];
                var q = 0;
                for (var I = 0; I < str.Length; I += 3)
                {
                    ret[q] = $"{double.Parse(str.Substring(I, 3))}";
                    q++;
                }

                return ret;
            }

            private static string GetCountStr(string str)
            {
                var ret = str;
                var len = (str.Length / 3 + 1) * 3 - str.Length;
                if (len < 3)
                {
                    for (var i = 0; i < len; i++)
                    {
                        ret = "0" + ret;
                    }
                }

                if (ret == "")
                    return "000";
                return ret;
            }

            private static string Convert_STR(string INT, int count)
            {
                string ret;
                //یک صد
                if (count == 0)
                {
                    if (INT.Substring(1, 1) == "1" && INT.Substring(2, 1) != "0")
                    {
                        ret = GET_Number(3, System.Convert.ToInt32(INT.Substring(0, 1)), " ") +
                              GET_Number(1, System.Convert.ToInt32(INT.Substring(2, 1)), "");
                    }
                    else
                    {
                        var str = GET_Number(0, System.Convert.ToInt32(INT.Substring(2, 1)), "");
                        ret = GET_Number(3, System.Convert.ToInt32(INT.Substring(0, 1)),
                                  GET_Number(2, System.Convert.ToInt32(INT.Substring(1, 1)), "") + str) +
                              GET_Number(2, System.Convert.ToInt32(INT.Substring(1, 1)), str) +
                              GET_Number(0, System.Convert.ToInt32(INT.Substring(2, 1)), "");
                    }
                }
                //هزار
                else if (count == 1)
                {
                    ret = Convert_STR(INT, 0);
                    ret += " " + _thousands[0];
                }
                //میلیون
                else if (count == 2)
                {
                    ret = Convert_STR(INT, 0);
                    ret += " " + _thousands[1];
                }
                //میلیارد
                else if (count == 3)
                {
                    ret = Convert_STR(INT, 0);
                    ret += " " + _thousands[2];
                }
                //تیلیارد
                else if (count == 4)
                {
                    ret = Convert_STR(INT, 0);
                    ret += " " + _thousands[3];
                }
                //بیلیارد
                else if (count == 5)
                {
                    ret = Convert_STR(INT, 0);
                    ret += " " + _thousands[4];
                }
                else
                {
                    ret = Convert_STR(INT, 0);
                    ret += count.ToString();
                }

                return ret;
            }

            private static string GET_Number(int count, int number, string va)
            {
                var ret = "";

                if (!string.IsNullOrEmpty(va))
                {
                    va = _and;
                }

                if (count == 0 || count == 1)
                {
                    var isDah = System.Convert.ToBoolean(count);
                    var myStr = new string[10];
                    myStr[1] = isDah ? _tenToTwenty[0] : _ones[0] + va;
                    myStr[2] = isDah ? _tenToTwenty[1] : _ones[1] + va;
                    myStr[3] = isDah ? _tenToTwenty[2] : _ones[2] + va;
                    myStr[4] = isDah ? _tenToTwenty[3] : _ones[3] + va;
                    myStr[5] = isDah ? _tenToTwenty[4] : _ones[4] + va;
                    myStr[6] = isDah ? _tenToTwenty[5] : _ones[5] + va;
                    myStr[7] = isDah ? _tenToTwenty[6] : _ones[6] + va;
                    myStr[8] = isDah ? _tenToTwenty[7] : _ones[7] + va;
                    myStr[9] = isDah ? _tenToTwenty[8] : _ones[8] + va;
                    return myStr[number];
                }

                if (count == 2)
                {
                    var myStr = new string[10];
                    myStr[1] = _tens[0];
                    myStr[2] = _tens[1] + va;
                    myStr[3] = _tens[2] + va;
                    myStr[4] = _tens[3] + va;
                    myStr[5] = _tens[4] + va;
                    myStr[6] = _tens[5] + va;
                    myStr[7] = _tens[6] + va;
                    myStr[8] = _tens[7] + va;
                    myStr[9] = _tens[8] + va;
                    return myStr[number];
                }

                if (count == 3)
                {
                    var myStr = new string[10];
                    myStr[1] = _hundreds[0] + va;
                    myStr[2] = _hundreds[1] + va;
                    myStr[3] = _hundreds[2] + va;
                    myStr[4] = _hundreds[3] + va;
                    myStr[5] = _hundreds[4] + va;
                    myStr[6] = _hundreds[5] + va;
                    myStr[7] = _hundreds[6] + va;
                    myStr[8] = _hundreds[7] + va;
                    myStr[9] = _hundreds[8] + va;
                    return myStr[number];
                }

                return ret;
            }

            #region Ones

            private static readonly string[] OnesFa =
            {
                "یک",
                "دو",
                "سه",
                "چهار",
                "پنج",
                "شش",
                "هفت",
                "هشت",
                "نه"
            };

            private static readonly string[] OnesEn =
            {
                "one",
                "two",
                "three",
                "four",
                "five",
                "six",
                "seven",
                "eight",
                "nine"
            };

            #endregion

            #region TenToTwenty

            private static readonly string[] TenToTwentyFa =
            {
                "یازده",
                "دوازده",
                "سیزده",
                "چهارده",
                "پانزده",
                "شانزده",
                "هفده",
                "هجده",
                "نوزده"
            };

            private static readonly string[] TenToTwentyEn =
            {
                "eleven",
                "twelve",
                "thirteen",
                "fourteen",
                "fifteen",
                "sixteen",
                "seventeen",
                "eighteen",
                "nineteen"
            };

            #endregion

            #region Tens

            private static readonly string[] TensFa =
            {
                "ده",
                "بیست",
                "سی",
                "چهل",
                "پنجاه",
                "شصت",
                "هفتاد",
                "هشتاد",
                "نود"
            };

            private static readonly string[] TensEn =
            {
                "ten",
                "twenty",
                "thirty",
                "forty",
                "fifty",
                "sixty",
                "seventy",
                "eighty",
                "ninety"
            };

            #endregion

            #region Hundreds

            private static readonly string[] HundredsFa =
            {
                "یکصد",
                "دویست",
                "سیصد",
                "چهارصد",
                "پانصد",
                "ششصد",
                "هفتصد",
                "هشتصد",
                "نهصد"
            };

            private static readonly string[] HundredsEn =
            {
                "one hundred",
                "two hundred",
                "three hundred",
                "four hundred",
                "five hundred",
                "six hundred",
                "seven hundred",
                "eight hundred",
                "nine hundred"
            };

            #endregion

            #region Thousands

            private static readonly string[] ThousandsFa =
            {
                "هزار",
                "میلیون",
                "میلیارد",
                "تیلیارد",
                "بیلیارد"
            };

            private static readonly string[] ThousandsEn =
            {
                "thousand",
                "million",
                "billion",
                "trillion",
                "quadrillion"
            };

            #endregion

            #region DecimalPlace

            private static readonly string[] DecimalFa =
            {
                "دهم",
                "صدم",
                "هزارم",
                "ده هزارم",
                "صدهزارم",
                "یک میلیونم",
                "ده میلیونم",
                "صد میلیونم",
                "هزار میلیونم",
                "یک میلیاردم",
                "ده میلیاردم",
                "صد میلیاردم",
                "هزار میلیاردم",
                "یک تیلیاردم",
                "ده تیلیاردم",
                "صد تیلیاردم",
                "هزار تیلیاردم",
                "یک بیلیاردم",
                "ده بیلیاردم",
                "صد بیلیاردم",
                "هزار بیلیاردم"
            };

            private static readonly string[] DecimalEn =
            {
                "tenths",
                "hundredths",
                "thousandths",
                "ten-thousandths",
                "hundred-thousandths",
                "millionths",
                "ten-millionths",
                "hundred-millionths",
                "billionths",
                "ten-billionths",
                "hundred-billionths",
                "trillionths",
                "ten-trillionths",
                "hundred-trillionths",
                "quadrillionths",
                "ten-quadrillionths",
                "hundred-quadrillionths",
                "quintillionths",
                "ten-quintillionths",
                "hundred-quintillionths",
                "sextillionths"
            };

            #endregion
        }
    }

    #endregion
}