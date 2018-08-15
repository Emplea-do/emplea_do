using System;
using Newtonsoft.Json;

namespace AppService.Framework.Social.LinkedIn
{
    public class UserInfo
    {
        public string id { get; set; }

        [JsonProperty("formattedName")]
        public string name { get; set; }

        [JsonProperty("emailAddress")]
        public string email { get; set; }
    }
}
