using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YueBa.Services
{
    // 时间字符串DateTime.Now.ToString("yyyy-MM-dd"))
    class EventServices
    {
        public static async Task<JObject> addEvent_NewPlace(String token, String name, String detail, String startTime, String endTime, String placeName, String address, String placeDetail, String price)
        {
            String jsonStr = JsonConvert.SerializeObject(new { token, name, detail, startTime, endTime, placeName, address, placeDetail, price });
            return await BasicService.postRequest("addEvent", jsonStr);
        }

        public static async Task<JObject> addEvent(String token, String name, String detail, String startTime, String endTime, String PlaceId)
        {
            String jsonStr = JsonConvert.SerializeObject(new { token, name, detail, startTime, endTime, PlaceId });
            return await BasicService.postRequest("addEvent", jsonStr);
        }

        public static async Task<JObject> deleteEvent(String token, String id)
        {
            String jsonStr = JsonConvert.SerializeObject(new { token, id });
            return await BasicService.postRequest("deleteEvent", jsonStr);
        }

        public static async Task<JObject> updateEvent(String token, String name, String detail, String startTime, String endTime, String PlaceId)
        {
            String jsonStr = JsonConvert.SerializeObject(new { token, name, detail, startTime, endTime, PlaceId });
            return await BasicService.postRequest("updateEvent", jsonStr);
        }

        public static async Task<JObject> updateEvent_NewPlace(String token, String name, String detail, String startTime, String endTime, String placeName, String address, String placeDetail, String price)
        {
            String jsonStr = JsonConvert.SerializeObject(new { token, name, detail, startTime, endTime, placeName, address, placeDetail, price });
            return await BasicService.postRequest("updateEvent", jsonStr);
        }

        public static async Task<JObject> getAllEvents()
        {
            return await BasicService.getRequest("getAllEvents");
        }

        public static async Task<JObject> getAllEventsParticipatesIn(String token)
        {
            String jsonStr = JsonConvert.SerializeObject(new { token });
            return await BasicService.postRequest("getParticipateEvents", jsonStr);
        }

        public static async Task<JObject> participateEvent(String token, String id)
        {
            String jsonStr = JsonConvert.SerializeObject(new { token, id });
            return await BasicService.postRequest("participate", jsonStr);
        }

        public static async Task<JObject> exitEvent(String token, String id)
        {
            String jsonStr = JsonConvert.SerializeObject(new { token, id });
            return await BasicService.postRequest("exitEvent", jsonStr);
        }
    }
}
