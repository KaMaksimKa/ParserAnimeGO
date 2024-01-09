using ParserAnimeGO.Models;

namespace ParserAnimeGO.Interface
{
    public interface IAnimeGoUriFactory
    {
        /// <summary>
        /// Получить url до страницы со всеми аниме
        /// </summary>
        /// <param name="animeArgs"></param>
        /// <returns></returns>
        Uri GetAnimes(AnimesArgs animeArgs);

        /// <summary>
        /// Получить url до информации о списках у пользователей для аниме
        /// </summary>
        /// <param name="animeId"></param>
        /// <returns></returns>
        Uri GetShowDataAnime(long animeId);

        /// <summary>
        /// Получить url до информации о озвучке у первой серии аниме
        /// </summary>
        /// <param name="animeId">id anime на сайте animego.org</param>
        /// <returns></returns>
        Uri GetEpisodeData(long animeId);

        /// <summary>
        /// Получить url до страницы с информацией о выходе новых серий
        /// </summary>
        /// <returns></returns>
        Uri GetAnimeNotifications();

        /// <summary>
        /// Получить комментарии по отдельному аниме
        /// </summary>
        /// <param name="idForComments"></param>
        /// <param name="numberOfPage"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Uri GetAnimeComments(long idForComments, int numberOfPage, int limit);

        /// <summary>
        /// Получить информацию об озвучках и плеерах по серии
        /// </summary>
        /// <param name="episodeId">id серии на сайте аниме го</param>
        /// <returns></returns>
        Uri GetEpisodeWatchData(long episodeId);
    }
}
