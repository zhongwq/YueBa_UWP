using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Net.Http;
using YueBa.Global;
using Windows.Web.Http;
using Windows.Storage.Streams;
using System.IO;

namespace YueBa.Services
{
    class PlaceServices
    {
        /***
         * 获取所有当前可用地点
         */
        public static async Task<Places> getAllPlaces()
        {
            var json = JsonSerializer.Create();
            String placesStr =  await BasicService.getRequest("getAllValidPlaces");
            return json.Deserialize<Places>(new JsonTextReader(new StringReader(placesStr)));
        }

        /***
         * 创建地点接口
         */
        public static async Task<bool> addPlace(String token, String name, String address, String detail, Double price, StorageFile file = null)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(token),"token");
            content.Add(new StringContent(name), "name");
            content.Add(new StringContent(address), "address");
            content.Add(new StringContent(detail), "detail");
            content.Add(new StringContent(price.ToString()), "price");
            
            if (file != null)
            {
                var streamData = await file.OpenStreamForReadAsync();
                var streamContent = new StreamContent(streamData);
                content.Add(streamContent, "image", file.Name);
            }
            var res = await BasicService.postRequestMultipartData("addPlace", content);

            return res != null;
        }

        /***
         * 更新地点信息
         */
        public static async Task<bool> updatePlace(String token, String id, String name, String address, String detail, Double price, StorageFile file = null)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(token), "token");
            content.Add(new StringContent(name), "name");
            content.Add(new StringContent(address), "address");
            content.Add(new StringContent(detail), "detail");
            content.Add(new StringContent(price.ToString()), "name");

            if (file != null)
            {
                var streamData = await file.OpenStreamForReadAsync();
                var streamContent = new StreamContent(streamData);
                content.Add(streamContent, "image", file.Name);
            }
            var res = await BasicService.postRequestMultipartData("updatePlace", content);

            return res != null;
        }

        /***
         * 删除地点信息
         */
        public static async Task<bool> deletePlace(String token, String id)
        {
            string jsonStr = JsonConvert.SerializeObject(new { token, id });
            var res = await BasicService.postRequestJSON("deletePlace", jsonStr);

            return res != null;
        }

        /***
         * 获取所有用户拥有的地点
         */
        public static async Task<Places> getAllOwnedPlace(String token)
        {
            string jsonStr = JsonConvert.SerializeObject(new { token });
            var json = JsonSerializer.Create();
            String placesStr = await BasicService.postRequestJSON("getAllOwnedPlaces", jsonStr);
            return json.Deserialize<Places>(new JsonTextReader(new StringReader(placesStr)));
        }

        /***
         * 获取Place详情
         */
        public static async Task<PlaceDetailClass> getPlaceDetail(String token, String id)
        {
            string jsonStr = JsonConvert.SerializeObject(new { token, id });
            var json = JsonSerializer.Create();
            String eventsStr = await BasicService.postRequestJSON("getDetailPlace", jsonStr);
            return (eventsStr == null) ? null : json.Deserialize<PlaceDetailClass>(new JsonTextReader(new StringReader(eventsStr)));
        }

        /***
         * 根据Id获取单个PlaceItem
         */
        public static async Task<PlaceItem> getSingleEvent(String id)
        {
            String jsonStr = JsonConvert.SerializeObject(new { id });
            String placeStr = await BasicService.postRequestJSON("getSinglePlace", jsonStr);
            var json = JsonSerializer.Create();
            return (placeStr == null) ? null : json.Deserialize<PlaceItem>(new JsonTextReader(new StringReader(placeStr)));
        }
    }
}
