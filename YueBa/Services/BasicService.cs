using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YueBa.Global;

namespace YueBa.Services
{
    class BasicService
    {
        public static async Task<JObject> getRequest(string request)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await httpClient.GetAsync(Config.api + request);
            string responseText = await response.Content.ReadAsStringAsync();
            return (JObject)JsonConvert.DeserializeObject(responseText);
        }

        public static async Task<JObject> postRequestJSON(string request, string postData)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await httpClient.PostAsync(Config.api+request, new StringContent(postData, Encoding.UTF8, "application/json"));
            string responseText = await response.Content.ReadAsStringAsync();
            return (JObject)JsonConvert.DeserializeObject(responseText);
        }

        public static async Task<JObject> postRequestMultipartData(string request, MultipartFormDataContent content) {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            HttpResponseMessage response = await httpClient.PostAsync(Config.api + request, content);
            string responseText = await response.Content.ReadAsStringAsync();
            return (JObject)JsonConvert.DeserializeObject(responseText);
        }
    }
}
