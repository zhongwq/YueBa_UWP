using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        public static async Task<JObject> Register(String username, String email, String password, String phone)
        {
            string jsonStr = JsonConvert.SerializeObject(new { username, email, password, phone });
            string responseText = await BasicService.postRequestJSON("register", jsonStr);
            return (responseText == null) ? null : (JObject)JsonConvert.DeserializeObject(responseText);
        }
    }
}
