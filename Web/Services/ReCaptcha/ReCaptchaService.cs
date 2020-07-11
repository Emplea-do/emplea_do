using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Web.Services.ReCaptcha
{
    public class ReCaptchaService : IReCaptchaService
    {
        private readonly string _recaptchaSiteKey;
        private readonly string _recaptchaSecret;

        public ReCaptchaService(IConfiguration configuration)
        {
            var RecaptchaSecret = configuration["GoogleReCaptcha:Secret"];
            _recaptchaSecret = RecaptchaSecret;
        }

        public bool ReCaptchaPassed(string gRecaptchaResponse)
        {
            HttpClient httpClient = new HttpClient();
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_recaptchaSecret}&response={gRecaptchaResponse}").Result;
            if (res.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            
            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);
            if (JSONdata.success != "true")
            {
                return false;
            }

            return true;
        }
    }

    public interface IReCaptchaService 
    {
        bool ReCaptchaPassed(string gRecaptchaResponse);
    }
}