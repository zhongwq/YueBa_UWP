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
        public static async Task<JObject> Login(String account, String password)
        {
            string jsonStr = JsonConvert.SerializeObject(new { account, password });
            return await BasicService.postRequestJSON("login", jsonStr);
        }

        public static async Task<JObject> Register(String username, String email, String password, String phone)
        {
            string jsonStr = JsonConvert.SerializeObject(new { username, email, password, phone });
            return await BasicService.postRequestJSON("register", jsonStr);
        }
    }
}
