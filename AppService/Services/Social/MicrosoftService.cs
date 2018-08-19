using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AppService.Framework;
using AppService.Framework.Social.Microsoft;
using Newtonsoft.Json;

namespace AppService.Services.Social
{
    public class MicrosoftService : IMicrosoftService
    {
        private string _clientId;
        private string _clientSecret;

        public MicrosoftService(string msClientId, string msClientSecret)
        {
            _clientId = msClientId;
            _clientSecret = msClientSecret;
        }
        public async Task<TaskResult<UserInfo>> GetUserInformation(string accessToken)
        {
            var result = new TaskResult<UserInfo>();
            var url = $"https://apis.live.net/v5.0/me";

            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var strResult = await client.GetStringAsync(url);
                    result.ExecutedSuccesfully = true;
                    result.Data = JsonConvert.DeserializeObject<UserInfo>(strResult);
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

        public async Task<TaskResult<AccessTokenResponse>> RequestTokenAsync(string code, string redirectUrl)
        {
            var result = new TaskResult<AccessTokenResponse>();
            var url = $"https://login.live.com/oauth20_token.srf";

            using (var client = new HttpClient())
            {
                try
                {
                    var postParams = new Dictionary<string, string>
                    {
                        {"client_id", _clientId},
                        {"scope", "wl.signin wl.basic"},
                        {"code", code},
                        {"redirect_uri", redirectUrl},
                        {"grant_type", "authorization_code"},
                        {"client_secret", _clientSecret},
                    };
                    var requestMessageContent = new FormUrlEncodedContent(postParams);
                    var strResult = await client.PostAsync(url, requestMessageContent);
                    result.ExecutedSuccesfully = true;
                    var resultContent = await strResult.Content.ReadAsStringAsync();
                    result.Data = JsonConvert.DeserializeObject<AccessTokenResponse>(resultContent);
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


    public interface IMicrosoftService 
    {
        Task<TaskResult<AccessTokenResponse>> RequestTokenAsync(string code, string redirectUrl);
        Task<TaskResult<UserInfo>> GetUserInformation(string accessToken);
    }
}
