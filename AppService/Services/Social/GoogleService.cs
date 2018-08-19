using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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

        public TaskResult<UserInfo> GetUserInformation(AccessTokenResponse data)
        {
            var result = new TaskResult<UserInfo>();
            try
            {
                var encodedStr = data.id_token.Split('.')[1];
                while (encodedStr.Length % 4 > 0)
                    encodedStr += "=";
                
                var buffer = Convert.FromBase64String(encodedStr);
                var str = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

                var decodedData = JsonConvert.DeserializeObject<dynamic>(str);

                result.ExecutedSuccesfully = true;
                result.Data = new UserInfo
                {
                    email = decodedData.email,
                    id = decodedData.sub,
                    name = decodedData.name
                };
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.AddErrorMessage(ex.Message);
                result.Data = null;
            }
            return result;
        }

        public async Task<TaskResult<AccessTokenResponse>> RequestTokenAsync(string code, string redirectUrl)
        {
            var result = new TaskResult<AccessTokenResponse>();
            var url = $"https://www.googleapis.com/oauth2/v4/token?";

            using (var client = new HttpClient())
            {
                try
                {
                    var postParams = new Dictionary<string, string>
                    {
                        {"code", code},
                        {"client_id", _clientId},
                        {"client_secret", _clientSecret},
                        {"redirect_uri", redirectUrl},
                        {"grant_type", "authorization_code"}
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
        TaskResult<UserInfo> GetUserInformation(AccessTokenResponse data);
    }
}
