using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using YueBa.Global;

namespace YueBa.Services
{
    class BasicService
    {
        public static async Task<String> getRequest(string request)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await httpClient.GetAsync(Config.api + request);
            String responseText = await response.Content.ReadAsStringAsync();
            if (response.StatusCode.ToString() != "OK")
            {
                var json = JsonSerializer.Create();
                ErrorItem error = json.Deserialize<ErrorItem>(new JsonTextReader(new StringReader(responseText)));
                new MessageDialog(error.error).ShowAsync();
                return null;
            }
            return responseText;
        }

        public static async Task<String> postRequestJSON(string request, string postData)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await httpClient.PostAsync(Config.api+request, new StringContent(postData, Encoding.UTF8, "application/json"));
            String responseText = await response.Content.ReadAsStringAsync();
            if (response.StatusCode.ToString() != "OK")
            {
                var json = JsonSerializer.Create();
                ErrorItem error = json.Deserialize<ErrorItem>(new JsonTextReader(new StringReader(responseText)));
                new MessageDialog(error.error).ShowAsync();
                return null;
            }
            return responseText;
        }

        public static async Task<String> postRequestMultipartData(string request, MultipartFormDataContent content) {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            HttpResponseMessage response = await httpClient.PostAsync(Config.api + request, content);
            String responseText = await response.Content.ReadAsStringAsync();
            if (response.StatusCode.ToString() != "OK")
            {
                var json = JsonSerializer.Create();
                ErrorItem error = json.Deserialize<ErrorItem>(new JsonTextReader(new StringReader(responseText)));
                new MessageDialog(error.error).ShowAsync();
                return null;
            }
            return responseText;
        }
    }
}
