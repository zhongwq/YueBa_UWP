using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YueBa.Global;

namespace YueBa.Services
{
    class PlaceServices
    {
        public static async Task<JObject> getAllPlaces()
        {
            return await BasicService.getRequest("getAllValidPlaces");
        }

        public static async Task<JObject> addPlace(String token, String name, String address, String detail, Double price)
        {
            String jsonStr = JsonConvert.SerializeObject(new { token, name, address, detail, price });
            return await BasicService.postRequest("addPlace", jsonStr);
        }

        public static async Task<JObject> updatePlace(String token, String id, String name, String address, String detail, Double price)
        {
            string jsonStr = JsonConvert.SerializeObject(new { token, id, name, address, detail, price });
            return await BasicService.postRequest("updatePlace", jsonStr);
        }

        public static async Task<JObject> deletePlace(String token, String id)
        {
            string jsonStr = JsonConvert.SerializeObject(new { token, id });
            return await BasicService.postRequest("deletePlace", jsonStr);
        }
    }
}
