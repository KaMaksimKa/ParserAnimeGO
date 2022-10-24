using ParserAnimeGO.Interface;

namespace ParserAnimeGO
{
    public class AnimeGoUriFactory: IAnimeGoUriFactory
    {
        private readonly string _defaultUrl  = $"https://animego.org/anime";
        public Uri GetAnimesByPageUri(int numberOfPage)
        {
            string url = _defaultUrl + $"?&page={numberOfPage}";
            return new Uri(url);
        }

        public Uri GetShowDataAnimeByIdFromAnimeGoUri(int idFromAnimeGo)
        {
            string url = $"https://animego.org/animelist/{idFromAnimeGo}/show";
            return new Uri(url);
        }

        public Uri GetVoiceoverDataAnimeByIdFromAnimeGoUri(int idFromAnimeGo)
        {
            string url = $"https://animego.org/anime/{idFromAnimeGo}/player?_allow=true";
            return new Uri(url);
        }

    }
}
