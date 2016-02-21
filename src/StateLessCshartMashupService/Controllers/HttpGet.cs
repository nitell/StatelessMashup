using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace StateLessCshartMashupService.Controllers
{
    public static class HttpGet
    {
        public static async Task<JObject> GetAsync(string url) => 
            Parse(await new HttpClient().GetAsync(url));
        

        private static JObject Parse(HttpResponseMessage response)
            => response.IsSuccessStatusCode ? JObject.Parse(response.Content.ReadAsStringAsync().Result) : null;
    }
}
