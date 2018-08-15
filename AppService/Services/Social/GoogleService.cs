using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AppService.Framework;
using AppService.Framework.Social.Google;
using Newtonsoft.Json;

namespace AppService.Services.Social
{
    public class GoogleService : IGoogleService
    {
        private string _clientId;
        private string _clientSecret;

        public GoogleService(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public async Task<TaskResult<UserInfo>> GetUserInformation(string accessToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TaskResult<AccessTokenResponse>> RequestTokenAsync(string code, string scope, string redirectUrl)
        {
            var result = new TaskResult<AccessTokenResponse>();
            var url = $"https://www.googleapis.com/oauth2/v4/token?grant_type=authorization_code&client_id={_clientId}&client_secret={_clientSecret}&scope={scope}&code={code}&redirect_uri={redirectUrl}";

            using (var client = new HttpClient())
            {
                try
                {
                    var postParams = new Dictionary<string, string>
                    {
                        {"", ""},
                        {"", ""},
                        {"", ""},
                        {"", ""},
                        {"", ""}
                    };
                    var requestMessageContent = new FormUrlEncodedContent(postParams);
                    var strResult = await client.PostAsync(url,requestMessageContent);
                    result.ExecutedSuccesfully = true;
                    var retsultContent = await strResult.Content.ReadAsStringAsync();
                    result.Data = JsonConvert.DeserializeObject<AccessTokenResponse>(retsultContent);
                }
                catch (Exception ex)
                {
                    result.Exception = ex;
                    result.AddErrorMessage(ex.Message);
                    result.Data = null;
                }
                return result;
            }
        }
    }

    public interface IGoogleService
    {

        Task<TaskResult<AccessTokenResponse>> RequestTokenAsync(string code, string redirectUrl);
        Task<TaskResult<UserInfo>> GetUserInformation(string accessToken);
    }
}
