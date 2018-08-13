using System;
namespace AppService.Framework.Social
{
    public class SocialKeys
    {
        public string LocalVerificationToken { get; set; }

        public string FacebookAppId { get; set; }
        public string FacebookAppSecret { get; set; }
        public string GoogleClientId { get; set; }
        public string GoogleClientSecret { get; set; }
        public string LinkedInClientId { get; set; }
        public string LinkedInClientSecret { get; set; }
        public string MsClientId { get; set; }
        public string MsClientSecret { get; set; }
        public string TwitterAccessToken { get; set; }
        public string TwitterAccessTokenSecret { get; set; }
        public string TwitterConsumerKey { get; set; }
        public string TwitterConsumerSecret { get; set; }

        public string GoogleMapsApiKey { get; set; }

        public string ReCaptchaPrivateKey { get; set; }
        public string ReCaptchaPublicKey { get; set; }

        public string SlackVerificationToken { get; set; }
        public string SlackWebhookEndpoint { get; set; }
    }
}
