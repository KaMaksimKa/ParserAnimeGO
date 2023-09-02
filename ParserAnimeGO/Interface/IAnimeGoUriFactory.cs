using ParserAnimeGO.Models;

namespace ParserAnimeGO.Interface
{
    public interface IAnimeGoUriFactory
    {
        public Uri GetAnimes(AnimesArgs animeArgs);
        public Uri GetShowDataAnime(long idFromAnimeGo);
        public Uri GetVoiceoverDataAnime(long idFromAnimeGo);
        public Uri GetAnimeNotifications();
        public Uri GetAnimeComments(long idForComments, int numberOfPage, int limit);
        public Uri GetAnimeImage(string imgIdFromAnimeGo);

    }

    

    
}
