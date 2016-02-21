using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static System.String;
using static StateLessCshartMashupService.Controllers.HttpGet;

namespace StateLessCshartMashupService.Controllers
{
    public static class Serve
    {
        private static async Task<string> GetCoverArt(string id) =>
            (await GetAsync(Format(Constants.CoverArtBaseUrl, id)))?.SelectToken("images[0].image")?.ToString();

        private static async Task<string> GetWiki(string id) => 
            (await GetAsync(Format(Constants.WikipediaBaseUrl, id)))?.SelectToken("query.pages.*.extract")?.ToString();

        private static string ScrapeWikiId(JObject o) => 
            ((string)o?.SelectTokens("relations[?(@.type == 'wikipedia')].url.resource")?.FirstOrDefault())?.Substring(29);

        private static IEnumerable<JToken> ScrapeAlbums(JObject o) => 
            o?.SelectTokens("release-groups[?(@.primary-type == 'Album')]") ?? Enumerable.Empty<JToken>();

        public static async Task<JObject> ServeMbid(string id)
        {
            var mb = await GetAsync(Format(Constants.MusicBrainzBaseUrl,id));
            var wiki = GetWiki(ScrapeWikiId(mb));
            var ca = ScrapeAlbums(mb).Select(x => Task.Run(async () => new { Title = x["title"], Art = await GetCoverArt((string)x["id"])})).ToArray();
            return mb != null ? new JObject
            {
                ["id"] = mb["id"],
                ["description"] = await wiki,
                ["albums"] = JArray.FromObject(await Task.WhenAll(ca))
            } : null;
        }
    }
}