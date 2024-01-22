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
        /// Получить url до информации о информации о эпизодах
        /// </summary>
        /// <param name="animeId">id anime на сайте animego.org</param>
        /// <returns></returns>
        Uri GetEpisodeData(long animeId);

        /// <summary>
        /// Получить информацию об озвучках и плеерах по аниме где нет деления на серии(полнометражные фильмы например)
        /// </summary>
        /// <param name="animeId">id anime на сайте animego.org</param>
        /// <returns></returns>
        Uri GetVideoDataFromFilm(long animeId);

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
        Uri GetVideoDatasFromEpisode(long episodeId);

        /// <summary>
        /// Получить заставку аниме
        /// </summary>
        /// <param name="imgIdFromAnimeGo"></param>
        /// <returns></returns>
        Uri GetAnimeImage(string imgIdFromAnimeGo);
    }
}
