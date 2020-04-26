using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Apex.Core.Helpers;
using Apex.Shared.Extensions;
using Newtonsoft.Json;
using RestSharp;

namespace Apex.Service.Services
{
    public class MessageService
    {
        private static RestClient Client => new RestClient("http://37.130.202.188/api/select");
        private static SmsParameter Param => new SmsParameter { UserName = "Tajik100", Password = "@Tajik100" };
        private static string FromNum => "100020400";


        public async Task<long> SendSmsAsync(string from, string[] to, string message)
        {
            var p = Param;
            //p.From = "50002040099420";
            //p.From = "5000125475";
            p.From = from.IsNeu(FromNum);
            p.To = to;
            p.Message = message;

            return await SendSmsAsync(p);
        }

        public async Task<long> SendSmsAsync(SmsParameter parameter)
        {
            parameter.To = parameter.To.Select(x => Regex.Replace(x, @".+(?=\d{10})", "")).Where(x => Regex.IsMatch(x, @"9\d{9}")).ToArray();
            if (!parameter.To.Any())
                return 0;
            parameter.Operator = SmsOperatorType.Send.Name();
            var request = new RestRequest(Method.POST);
            SetHeaders(request);
            request.AddParameter("undefined", parameter.Stringify(), ParameterType.RequestBody);
            var response = await Client.ExecuteTaskAsync(request);
            var data = JsonConvert.DeserializeObject<List<long>>(response.Content);
            var status = data.Count > 0 && data[0] == 0;
            return status ? data[1] : 0;
        }

        public async Task SendOperatorPassAsync(string to, string name, string password, string expire)
        {
            to = Regex.Replace(to, @".+(?=\d{10})", "");

            if (!Regex.IsMatch(to, @"9\d{9}"))
                return;
            //var json = new
            //{
            //    op = "pattern",
            //    user = Param.UserName,
            //    pass = Param.Password,
            //    fromNum = FromNum,
            //    toNum = to,
            //    patternCode = "1220",
            //    inputData = new object[]
            //    {
            //        new {name},
            //        new {pass=password},
            //        new {expire},
            //    }
            //};
            var str =
                "{\"op\":\"pattern\",\"user\":\"Tajik100\",\"pass\":\"@Tajik100\",\"fromNum\":\"100020400\",\"toNum\":\"9178574885\",\"patternCode\":\"1220\",\"inputData\":[{\"name\":\"احمد روح بخش بهحانی\"},{\"pass\":\"e6GxN0\"},{\"expire\":\"1397/12/03 13:45\"}]}";
            //var str = json.Stringify();
            var request = new RestRequest(Method.POST);
            SetHeaders(request);
            request.AddParameter("undefined", str, ParameterType.RequestBody);
            var response = await Client.ExecuteTaskAsync(request);
            //var json = JsonConvert.DeserializeObject<List<long>>(response.Content);
            //var status = data.Count > 0 && data[0] == 0;
            //return status ? data[1] : 0;
        }

        public async Task<decimal> GetCreditAsync()
        {
            return await GetCreditAsync(Param);
        }
        public async Task<decimal> GetCreditAsync(SmsParameter parameter)
        {
            parameter.Operator = SmsOperatorType.Credit.Name();
            var request = new RestRequest(Method.POST);
            SetHeaders(request);
            request.AddParameter("undefined", parameter.Stringify(), ParameterType.RequestBody);
            var response = await Client.ExecuteTaskAsync(request);
            var data = JsonConvert.DeserializeObject<List<decimal>>(response.Content);
            var status = data.Count > 0 && data[0] == 0;
            return status ? data[1] : 0;
        }


        private static void SetHeaders(IRestRequest request)
        {
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
        }
    }

    public class SmsParameter
    {
        [JsonProperty(PropertyName = "op")]
        public string Operator { get; set; }
        [JsonProperty(PropertyName = "uname")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "pass")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "from")]
        public string From { get; set; }
        [JsonProperty(PropertyName = "to")]
        public string[] To { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
    public enum SmsOperatorType
    {
        [Display(Name = "send")]
        Send = 0,
        [Display(Name = "credit")]
        Credit = 1
    }
}