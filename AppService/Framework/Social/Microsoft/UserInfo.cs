using System;
namespace AppService.Framework.Social.Microsoft
{
    public class UserInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public dynamic emails { get; set; }
    }
}
