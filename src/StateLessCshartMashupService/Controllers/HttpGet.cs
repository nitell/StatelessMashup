using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Microsoft.AspNet.Razor;

namespace StateLessCshartMashupService.Controllers
{
    public static class HttpGet
    {
        public static JObject Get(string url)
        {            
            return ParseResults(new HttpClient().GetAsync(url).Result);            
        }

        private static JObject ParseResults(HttpResponseMessage result)
        {
            return result.IsSuccessStatusCode ? JObject.Parse(result.Content.ReadAsStringAsync().Result) : null;
        }
    }
}
