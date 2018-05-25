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

namespace YueBa.Services
{
    // 时间字符串示例DateTime.Now.ToString("yyyy-MM-dd"))
    class EventServices
    {
        /***
         * 创建事件接口, 可同时创建Place
         */
        public static async Task<JObject> addEvent_NewPlace(String token, String name, String detail, String startTime, String endTime, String placeName, String address, String placeDetail, String price, int maxNum, StorageFile file = null)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(token), "token");
            content.Add(new StringContent(name), "name");
            content.Add(new StringContent(detail), "detail");
            content.Add(new StringContent(startTime), "startTime");
            content.Add(new StringContent(endTime), "endTime");
            content.Add(new StringContent(placeName), "placeName");
            content.Add(new StringContent(placeDetail), "placeDetail");
            content.Add(new StringContent(address), "address");
            content.Add(new StringContent(maxNum.ToString()), "maxNum");
            content.Add(new StringContent(price.ToString()), "price");

            if (file != null)
            {
                var streamData = await file.OpenStreamForReadAsync();
                var streamContent = new StreamContent(streamData);
                content.Add(streamContent, "image", file.Name);
            }
            return await BasicService.postRequestMultipartData("addEvent", content);
        }

        /***
         * 创建Event接口
         */
        public static async Task<JObject> addEvent(String token, String name, String detail, String startTime, String endTime, String PlaceId, int maxNum, StorageFile file = null)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(token), "token");
            content.Add(new StringContent(name), "name");
            content.Add(new StringContent(detail), "detail");
            content.Add(new StringContent(startTime), "startTime");
            content.Add(new StringContent(endTime), "endTime");
            content.Add(new StringContent(PlaceId), "placeId");
            content.Add(new StringContent(maxNum.ToString()), "maxNum");

            if (file != null)
            {
                var streamData = await file.OpenStreamForReadAsync();
                var streamContent = new StreamContent(streamData);
                content.Add(streamContent, "image", file.Name);
            }
            return await BasicService.postRequestMultipartData("addEvent", content);
        }

        /***
         * 删除Event接口
         */
        public static async Task<JObject> deleteEvent(String token, String id)
        {
            String jsonStr = JsonConvert.SerializeObject(new { token, id });
            return await BasicService.postRequestJSON("deleteEvent", jsonStr);
        }

        /***
         * 更新Event接口
         */
        public static async Task<JObject> updateEvent(String token, String id, String name, String detail, String startTime, String endTime, String PlaceId, int maxNum, StorageFile file = null)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(id), "id");
            content.Add(new StringContent(token), "token");
            content.Add(new StringContent(name), "name");
            content.Add(new StringContent(detail), "detail");
            content.Add(new StringContent(startTime), "startTime");
            content.Add(new StringContent(endTime), "endTime");
            content.Add(new StringContent(PlaceId), "placeId");
            content.Add(new StringContent(maxNum.ToString()), "maxNum");

            if (file != null)
            {
                var streamData = await file.OpenStreamForReadAsync();
                var streamContent = new StreamContent(streamData);
                content.Add(streamContent, "image", file.Name);
            }
            return await BasicService.postRequestMultipartData("updateEvent", content);
        }

        /***
         * 更新Event，可创建地点
         */
        public static async Task<JObject> updateEvent_NewPlace(String token, String id, String name, String detail, String startTime, String endTime, String placeName, String address, String placeDetail, String price, int maxNum, StorageFile file = null)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(id), "id");
            content.Add(new StringContent(token), "token");
            content.Add(new StringContent(name), "name");
            content.Add(new StringContent(detail), "detail");
            content.Add(new StringContent(startTime), "startTime");
            content.Add(new StringContent(endTime), "endTime");
            content.Add(new StringContent(placeName), "placeName");
            content.Add(new StringContent(placeDetail), "placeDetail");
            content.Add(new StringContent(address), "address");
            content.Add(new StringContent(maxNum.ToString()), "maxNum");
            content.Add(new StringContent(price.ToString()), "price");

            if (file != null)
            {
                var streamData = await file.OpenStreamForReadAsync();
                var streamContent = new StreamContent(streamData);
                content.Add(streamContent, "image", file.Name);
            }

            return await BasicService.postRequestMultipartData("addEvent", content);
        }

        /***
         * 获取所有Event
         */
        public static async Task<JObject> getAllEvents()
        {
            return await BasicService.getRequest("getAllEvents");
        }

        /***
         * 获取所有参与的Event，不包括自己创建的
         */
        public static async Task<JObject> getAllEventsParticipatesIn(String token)
        {
            String jsonStr = JsonConvert.SerializeObject(new { token });
            return await BasicService.postRequestJSON("getParticipateEvents", jsonStr);
        }

        /***
         * 获取所有自己创建的Event
         */
        public static async Task<JObject> getAllOwnedEvents(String token)
        {
            string jsonStr = JsonConvert.SerializeObject(new { token });
            return await BasicService.postRequestJSON("getAllOwnedEvents", jsonStr);
        }

        /***
         * 参与事件的接口
         */
        public static async Task<JObject> participateEvent(String token, String id)
        {
            String jsonStr = JsonConvert.SerializeObject(new { token, id });
            return await BasicService.postRequestJSON("participate", jsonStr);
        }

        /***
         * 退出事件的接口
         */
        public static async Task<JObject> exitEvent(String token, String id)
        {
            String jsonStr = JsonConvert.SerializeObject(new { token, id });
            return await BasicService.postRequestJSON("exitEvent", jsonStr);
        }
    }
}
