using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace YueBa.Services
{
    class AuthServices
    {
        /***
         * 登陆接口
         */
        public static async Task<JObject> Login(String account, String password)
        {
            string jsonStr = JsonConvert.SerializeObject(new { account, password });
            string responseText = await BasicService.postRequestJSON("login", jsonStr);
            return (responseText == null) ? null : (JObject)JsonConvert.DeserializeObject(responseText);
        }

        /***
         * 注册
         */
        public static async Task<JObject> Register(String username, String email, String password, String phone, StorageFile file = null)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(username), "username");
            content.Add(new StringContent(email), "email");
            content.Add(new StringContent(password), "password");
            content.Add(new StringContent(phone), "phone");

            if (file != null)
            {
                var streamData = await file.OpenStreamForReadAsync();
                var streamContent = new StreamContent(streamData);
                content.Add(streamContent, "image", file.Name);
            }
            var res = await BasicService.postRequestMultipartData("register", content);

            return (res == null) ? null : (JObject)JsonConvert.DeserializeObject(res);
        }
    }
}
