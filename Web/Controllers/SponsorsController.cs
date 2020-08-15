using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public class SponsorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public string GetOpenCollective(string members)
        {
            // It's not call directly from AJAX due to CORS.
            var resultString = WebRequestHelper("https://opencollective.com/emplea_do/members/" + members);
            //var resultObject = JsonConvert.DeserializeObject<dynamic>(resultString);

            return resultString;
        }

        public string WebRequestHelper(string endpoint)
        {

            WebRequest request = WebRequest.Create(endpoint);

            WebResponse response = request.GetResponse();

            string responseFromServer = string.Empty;

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
            }
            response.Close();

            return responseFromServer;
        }
    }
}