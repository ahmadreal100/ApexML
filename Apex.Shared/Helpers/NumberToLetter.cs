using System.Text.RegularExpressions;

namespace Apex.Shared.Helpers
{
    public static class NumberToLetter
    {
        /// <summary>
        /// Convert number to persian string.
        /// </summary>
        /// <param name="number">number to convert.</param>
        /// <returns></returns>
        public static string Convert(double number)
        {
            var digit = number.ToString("0." + new string('#', 21));
            if (double.Parse(digit).Equals(0))
                return "صفر";
            var isNegative = digit.Contains("-");
            var hasDecimal = digit.Contains(".");
            var entireArray = Regex.Split(Regex.Replace(digit, "-", ""), "\\D");
            var round = entireArray[0];

            var ret = CycleToConvert(round);
            var n = "";
            if (hasDecimal)
                n = Regex.Replace(double.Parse(entireArray[1].Insert(0, ".")).ToString("0." + new string('#', 21)), "\\d+\\.", string.Empty);
            return $"{(isNegative ? "منفی " : "")}{ret.Trim()}{(hasDecimal ? $"{(ret.Trim() == "صفر" || string.IsNullOrEmpty(ret) ? "" : " و ")}{BuildDecimalPlaceString(n).Trim()}" : "")}";
        }

        private static string BuildDecimalPlaceString(string num)
        {
            var ret = CycleToConvert(num);
            return ret != "صفر" ? $"{ret.Trim()} {DecimalPlaceTitle[num.Length - 1]}" : "";
        }

        private static string CycleToConvert(string num)
        {
            var ret = "";
            var mainStr = STR_To_Int(num);
            var q = 0;
            for (var i = mainStr.Length - 1; i >= 0; i--)
            {
                var strva = string.IsNullOrWhiteSpace(ret) ? " " : " و ";
                var ns = Convert_STR(GetCountStr(mainStr[i]), q);
                ret = (double.Parse(mainStr[i]) > 0 ? ns + strva : "") + ret;
                q++;
            }
            if (ret == " " || ret == "  ")
                ret = "صفر";
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
                    ret = GET_Number(3, System.Convert.ToInt32(INT.Substring(0, 1)), " ") + GET_Number(1, System.Convert.ToInt32(INT.Substring(2, 1)), "");
                }
                else
                {
                    var str = GET_Number(0, System.Convert.ToInt32(INT.Substring(2, 1)), "");
                    ret = GET_Number(3, System.Convert.ToInt32(INT.Substring(0, 1)), GET_Number(2, System.Convert.ToInt32(INT.Substring(1, 1)), "") + str) + GET_Number(2, System.Convert.ToInt32(INT.Substring(1, 1)), str) + GET_Number(0, System.Convert.ToInt32(INT.Substring(2, 1)), "");
                }
            }
            //هزار
            else if (count == 1)
            {
                ret = Convert_STR(INT, 0);
                ret += " هزار";
            }
            //میلیون
            else if (count == 2)
            {
                ret = Convert_STR(INT, 0);
                ret += " میلیون";
            }
            //میلیارد
            else if (count == 3)
            {
                ret = Convert_STR(INT, 0);
                ret += " میلیارد";
            }
            //میلیارد
            else if (count == 4)
            {
                ret = Convert_STR(INT, 0);
                ret += " تیلیارد";
            }
            //میلیارد
            else if (count == 5)
            {
                ret = Convert_STR(INT, 0);
                ret += " بیلیارد";
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
                va = " و ";
            }
            if (count == 0 || count == 1)
            {
                var isDah = System.Convert.ToBoolean(count);
                var myStr = new string[10];
                myStr[1] = isDah ? "یازده" : "یک" + va;
                myStr[2] = isDah ? "دوازده" : "دو" + va;
                myStr[3] = isDah ? "سیزده" : "سه" + va;
                myStr[4] = isDah ? "چهارده" : "چهار" + va;
                myStr[5] = isDah ? "پانزده" : "پنج" + va;
                myStr[6] = isDah ? "شانزده" : "شش" + va;
                myStr[7] = isDah ? "هفده" : "هفت" + va;
                myStr[8] = isDah ? "هجده" : "هشت" + va;
                myStr[9] = isDah ? "نوزده" : "نه" + va;
                return myStr[number];
            }
            if (count == 2)
            {
                var myStr = new string[10];
                myStr[1] = "ده";
                myStr[2] = "بیست" + va;
                myStr[3] = "سی" + va;
                myStr[4] = "چهل" + va;
                myStr[5] = "پنجاه" + va;
                myStr[6] = "شصت" + va;
                myStr[7] = "هفتاد" + va;
                myStr[8] = "هشتاد" + va;
                myStr[9] = "نود" + va;
                return myStr[number];
            }
            if (count == 3)
            {
                var myStr = new string[10];
                myStr[1] = "یکصد" + va;
                myStr[2] = "دویست" + va;
                myStr[3] = "سیصد" + va;
                myStr[4] = "چهارصد" + va;
                myStr[5] = "پانصد" + va;
                myStr[6] = "ششصد" + va;
                myStr[7] = "هفتصد" + va;
                myStr[8] = "هشتصد" + va;
                myStr[9] = "نهصد" + va;
                return myStr[number];
            }
            return ret;
        }

        #region DecimalPlaceNumber
        private static readonly string[] DecimalPlaceTitle = {
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
        #endregion
    }
}