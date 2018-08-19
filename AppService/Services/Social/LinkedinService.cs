using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AppService.Framework;
using AppService.Framework.Social.LinkedIn;
using Newtonsoft.Json;

namespace AppService.Services.Social
{
    public class LinkedinService : ILinkedinService
    {
        private string _clientId;
        private string _clientSecret;
        public LinkedinService(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public async Task<TaskResult<UserInfo>> GetUserInformation(string accessToken)
        {
            var result = new TaskResult<UserInfo>();
            var url = $"https://api.linkedin.com/v1/people/~:(email-address,formatted-name,id)?format=json";

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
            var url = $"https://www.linkedin.com/oauth/v2/accessToken?grant_type=authorization_code&client_id={_clientId}&client_secret={_clientSecret}&code={code}&redirect_uri={redirectUrl}";

            using (var client = new HttpClient())
            {
                try
                {
                    var strResult = await client.GetStringAsync(url);
                    result.ExecutedSuccesfully = true;
                    result.Data = JsonConvert.DeserializeObject<AccessTokenResponse>(strResult);
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

    public interface ILinkedinService
    {
        Task<TaskResult<AccessTokenResponse>> RequestTokenAsync(string code, string redirectUrl);
        Task<TaskResult<UserInfo>> GetUserInformation(string accessToken);
    }
}
