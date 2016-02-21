using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace StateLessCshartMashupService.Controllers
{
    public static class HttpGet
    {
        public static async Task<JObject> GetAsync(string url)
        {
            var response = await new HttpClient().GetAsync(url);
            return response.IsSuccessStatusCode ? JObject.Parse(response.Content.ReadAsStringAsync().Result) : null;
        }
    }
}
