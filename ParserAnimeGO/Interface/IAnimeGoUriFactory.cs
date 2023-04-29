using ParserAnimeGO.Models;

namespace ParserAnimeGO.Interface
{
    public interface IAnimeGoUriFactory
    {
        public Uri GetAnimes(AnimesArgs animeArgs);
        public Uri GetShowDataAnimeById(int idFromAnimeGo);
        public Uri GetVoiceoverDataAnimeById(int idFromAnimeGo);
        public Uri GetAnimeNotifications();

    }

    

    
}
