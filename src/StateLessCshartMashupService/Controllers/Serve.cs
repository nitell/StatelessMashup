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

        private static Task<JToken>[] GetAlbums(IEnumerable<JToken> o) =>
            o.Select(x => Task.Run(async () => (JToken)new JObject{["title"] = x["title"], ["art"] = await GetCoverArt((string) x["id"])})).ToArray();
        
        private static async Task<string> GetWiki(string id) => 
            (await GetAsync(Format(Constants.WikipediaBaseUrl, id)))?.SelectToken("query.pages.*.extract")?.ToString();

        private static string ScrapeWikiId(JToken o) => 
            ((string)o?.SelectTokens("relations[?(@.type == 'wikipedia')].url.resource")?.FirstOrDefault())?.Substring(29);

        private static IEnumerable<JToken> ScrapeAlbums(JToken o) => 
            o?.SelectTokens("release-groups[?(@.primary-type == 'Album')]") ?? Enumerable.Empty<JToken>();

        public static async Task<JObject> ServeArtist(string id) =>
            await Search(await GetAsync(Format(Constants.MusicBrainzBaseUrl, id)));

        public static async Task<JObject> Search(JToken mb) => 
            mb != null ? await Fix(mb["id"], GetWiki(ScrapeWikiId(mb)), GetAlbums(ScrapeAlbums(mb))) : null;

        public static async Task<JObject> Fix(JToken id, Task<string> wikiTask, Task<JToken>[] albumTasks) => new JObject {
                ["id"] = id,
                ["description"] = await wikiTask,
                ["albums"] = JArray.FromObject(await Task.WhenAll(albumTasks))
            };
    }
}