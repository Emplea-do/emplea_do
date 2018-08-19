using System;
using System.Net.Http;
using System.Threading.Tasks;
using AppService.Framework;
using AppService.Framework.Social.Facebook;
using Newtonsoft.Json;

namespace AppService.Services.Social
{
    public class FaceBookService : BaseService, IFacebookService
    {
        private string _appId;
        private string _appSecret;

        public FaceBookService(string appId, string secret)
        {
            _appId = appId;
            _appSecret = secret;
        }

        public async Task<TaskResult<UserInfo>> GetUserInformation(string accessToken)
        {
            var result = new TaskResult<UserInfo>();
            var url = $"https://graph.facebook.com/v3.1/me/?access_token={accessToken}&fields=id,name,email";

            using (var client = new HttpClient())
            {
                try
                {
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

        public async Task<TaskResult<TokenResponse>> ValidateCode(string code, string redirectUrl)
        {
            var result = new TaskResult<TokenResponse>();
            var url = $"https://graph.facebook.com/v3.1/oauth/access_token?client_id={_appId}&client_secret={_appSecret}&code={code}&redirect_uri={redirectUrl}";

            using (var client = new HttpClient())
            {
                try
                {
                    var strResult = await client.GetStringAsync(url);
                    result.ExecutedSuccesfully = true;
                    result.Data = JsonConvert.DeserializeObject<TokenResponse>(strResult);
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

    public interface IFacebookService 
    {
        Task<TaskResult<TokenResponse>> ValidateCode(string code, string redirectUrl);
        Task<TaskResult<UserInfo>> GetUserInformation(string accessToken);
    }
}
