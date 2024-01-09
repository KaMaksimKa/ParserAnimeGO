using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ParserAnimeGO.Models.ParserModels;

namespace ParserAnimeGO.Interface
{
    public interface IAnimeParserFromIDocument
    {
        /// <summary>
        /// Получение частичной информации об аниме со страницы с несколькоми аниме с сайта AnimeGO:
        /// Href,AnimeId,TitleEn,TitleRu,Description,Type,Year
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        List<PartialAnimeData> GetPartialAnime(IDocument document);

        /// <summary>
        /// Получение основной информации об аниме со страницы аниме с сайта AnimeGO:
        /// Rate, CountEpisode, Status, Genres, MpaaRate, Studios,Duration,NextEpisode,Dubbing,ImgHref
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        MainAnimeData? GetMainDataAnime(IDocument document);

        /// <summary>
        /// Получение  информации об просмотрах у одного аниме:
        /// Completed, Planned, Dropped, OnHold, Watching
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        ShowAnimeData? GetShowDataAnime(IDocument document);

        /// <summary>
        /// Получение информации о выходе новых серий
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        List<AnimeNotificationData> GetAnimeNotificationsData(IDocument document);

        /// <summary>
        /// Получение комментариев по аниме
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        List<AnimeCommentData> GetAnimeCommentsData(IDocument document);

        /// <summary>
        /// Получение информации об сериях в аниме
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        List<EpisodeData> GetEpisodesData(IDocument document);

        /// <summary>
        /// Получение подробных данных об одной серии
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        List<VideoData> GetVideoDatas(IDocument document);
    }
}
