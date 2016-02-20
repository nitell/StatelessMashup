using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace StateLessCshartMashupService.Controllers
{
    public static class Serve
    {
        private static dynamic ScrapeMusicBrainzResponse(JObject o)
        {
            return o == null
                ? null
                : new
                {
                    Id = o["id"],
                    Wikipedia = o.SelectTokens("relations[?(@.type == 'wikipedia')].url.resource").FirstOrDefault(),
                    Albums = o.SelectTokens("release-groups[?(@.primary-type == 'Album')]")
                        .Select(a => new {Id = a["id"], Title = a["title"]})
                        .ToArray()
                };
        }

        private static dynamic GetCoverArt(dynamic mbId)
        {
            return HttpGet.Get(string.Format(Constants.CoverArtBaseUrl, mbId));
        }

        private static dynamic GetWiki(dynamic wikiUrl)
        {
            return
                HttpGet.Get(string.Format(Constants.WikipediaBaseUrl,
                    wikiUrl.ToString().Substring("http://en.wikipedia.org/wiki/".Length)));
        }

        private static dynamic GetMusicBrainz(dynamic mbId)
        {
            return HttpGet.Get(string.Format(Constants.MusicBrainzBaseUrl, mbId));
        }

        private static dynamic ScrapeWikipedia(dynamic res)
        {
            return res.SelectToken("query.pages.*.extract");
        }

        private static dynamic ScrapeCoverArt(dynamic res)
        {
            return res?.SelectToken("images[0].image");
        }

        private static async Task<dynamic> Search(dynamic mbResponse)
        {
            return mbResponse == null
                ? null
                : new
                {
                    mbResponse.Id,
                    ScrapeWikipedia = await Task.Run(() => ScrapeWikipedia(GetWiki(mbResponse.Wikipedia))),
                    Albums =
                        await
                            Task.WhenAll(
                                ((IEnumerable<dynamic>) mbResponse.Albums).Select(
                                    a => Task.Factory.StartNew(() =>
                                        new {a.Title, Image = ScrapeCoverArt(GetCoverArt(a.Id))})).ToArray())
                };
        }

        public static Task<dynamic> ServeMbid(string mbId)
        {
            return Search(ScrapeMusicBrainzResponse(GetMusicBrainz(mbId)));
        }
    }
}