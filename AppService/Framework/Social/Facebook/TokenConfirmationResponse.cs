using System;
namespace AppService.Framework.Social.Facebook
{
    public class TokenConfirmationResponse
    {

        /* https://developers.facebook.com/docs/facebook-login/manually-build-a-login-flow/#checktoken
    "data": {
        "app_id": 138483919580948, 
        "type": "USER",
        "application": "Social Cafe", 
        "expires_at": 1352419328, 
        "is_valid": true, 
        "issued_at": 1347235328, 
        "metadata": {
            "sso": "iphone-safari"
        }, 
        "scopes": [
            "email", 
            "publish_actions"
        ], 
        "user_id": "1207059"
    }
        */

        TokenConfirmationResponseData Data { get; set; }
    }

    public class TokenConfirmationResponseData
    {
        public string app_id { get; set; }
        public string type { get; set; }
        public string application { get; set; }
        public string expires_at { get; set; }
        public string is_valid { get; set; }
        public string issued_at { get; set; } 
        public string metadata { get; set; }
        public string[] scopes { get; set; }

        public string user_id { get; set; }
    }
}
