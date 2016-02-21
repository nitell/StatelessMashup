namespace StateLessCshartMashupService.Controllers
{
    public static class Constants
    {
        public const string MusicBrainzBaseUrl = "http://musicbrainz.org/ws/2/artist/{0}?&fmt=json&inc=url-rels+release-groups";
        public const string CoverArtBaseUrl = "http://coverartarchive.org/release-group/{0}";
        public const string WikipediaBaseUrl = "https://en.wikipedia.org/w/api.php?action=query&format=json&prop=extracts&exintro=true&redirects=true&titles={0}";
    }
}
