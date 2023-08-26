using ParserAnimeGO.Models;

namespace ParserAnimeGO.Interface
{
    public interface IAnimeGoUriFactory
    {
        public Uri GetAnimes(AnimesArgs animeArgs);
        public Uri GetShowDataAnimeById(long idFromAnimeGo);
        public Uri GetVoiceoverDataAnimeById(long idFromAnimeGo);
        public Uri GetAnimeNotifications();
        public Uri GetAnimeComments(long idForComments, int numberOfPage, int limit);

    }

    

    
}
