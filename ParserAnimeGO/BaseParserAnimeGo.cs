using ParserAnimeGO.Interface;
using ParserAnimeGO.Models;
using ParserAnimeGO.Models.ParserModels;
using System.Collections.Generic;

namespace ParserAnimeGO
{
    public class BaseParserAnimeGo : IDisposable
    {
        private readonly IRequestParserHandler _requestParserHandler;
        private readonly IRequestParserFactory _requestParserFactory;
        private readonly IAnimeParserFromIDocument _parserFromIDocument;
        private readonly IAnimeGoUriFactory _uriFactory;
        public BaseParserAnimeGo(IRequestParserHandler requestParserHandler,
            IRequestParserFactory requestParserFactory,
            IAnimeParserFromIDocument parserFromIDocument,
            IAnimeGoUriFactory uriFactory)
        {
            _requestParserHandler = requestParserHandler;
            _requestParserFactory = requestParserFactory;
            _parserFromIDocument = parserFromIDocument;
            _uriFactory = uriFactory;
        }


        /// <summary>
        /// Получить поверхностную информацию со страниц с аниме
        /// </summary>
        /// <param name="animesArgs"></param>
        /// <returns></returns>
        public async Task<List<PartialAnimeData>> GetPartialAnimesDataByArgsAsync(AnimesArgs animesArgs)
        {
            using var requestMessage = _requestParserFactory
                .GetHtmlRequestMessage(_uriFactory.GetAnimes(animesArgs));
            using var document = await _requestParserHandler.SendHtmlRequestAsync(requestMessage);

            return _parserFromIDocument.GetPartialAnime(document);
        }

        /// <summary>
        /// Получить подробную информацию об отдельном аниме
        /// </summary>
        /// <param name="animeHref"></param>
        /// <returns></returns>
        public async Task<MainAnimeData?> GetMainAnimeDataByAnimeHrefGoAsync(string animeHref)
        {
            using var requestMessage = _requestParserFactory.GetHtmlRequestMessage(new Uri(animeHref));
            using var document = await _requestParserHandler.SendHtmlRequestAsync(requestMessage);

            return _parserFromIDocument.GetMainDataAnime(document);
        }

        /// <summary>
        /// Получить информацию о просмотрах об отдельном аниме
        /// </summary>
        /// <param name="animeId"></param>
        /// <returns></returns>
        public async Task<ShowAnimeData?> GetShowAnimeDataByIdFromAnimeGoAsync(long animeId)
        {
            using var requestMessage = _requestParserFactory.GetJsonRequestMessage(
                _uriFactory.GetShowDataAnime(animeId));
            using var document = await _requestParserHandler.SendJsonRequestAsync(requestMessage);

            var showAnimeData = _parserFromIDocument.GetShowDataAnime(document);

            if (showAnimeData != null)
            {
                showAnimeData.AnimeId = animeId;
            }


            return showAnimeData;
        }

        /// <summary>
        /// Получить информацию об выходе новых серий
        /// </summary>
        /// <returns></returns>
        public async Task<List<AnimeNotificationData>> GetAnimeNotificationsFromAnimeGoAsync()
        {
            var uri = _uriFactory.GetAnimeNotifications();
            var request = _requestParserFactory.GetHtmlRequestMessage(uri);
            var document = await _requestParserHandler.SendHtmlRequestAsync(request);

            return _parserFromIDocument.GetAnimeNotificationsData(document);
        }

        /// <summary>
        /// Получить комментарии по аниме
        /// </summary>
        /// <param name="idForComments"></param>
        /// <param name="numberOfPage"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<AnimeCommentData>> GetAnimeCommentsAsync(long idForComments, int numberOfPage = 1, int limit = 20)
        {
            var uri = _uriFactory.GetAnimeComments(idForComments, numberOfPage,limit);
            var request = _requestParserFactory.GetJsonRequestMessage(uri);
            var document = await _requestParserHandler.SendJsonRequestAsync(request);

            return _parserFromIDocument.GetAnimeCommentsData(document);
        }

        /// <summary>
        /// Получение информации об сериях в аниме
        /// </summary>
        /// <param name="animeId"></param>
        /// <returns></returns>
        public async Task<List<EpisodeData>> GetEpisodesDataAsync(long animeId)
        {
            var uri = _uriFactory.GetEpisodeData(animeId);
            var request = _requestParserFactory.GetJsonRequestMessage(uri);
            var document = await _requestParserHandler.SendJsonRequestAsync(request);

            var episodesData = _parserFromIDocument.GetEpisodesData(document);

            foreach (var episode in episodesData)
            {
                episode.AnimeId = animeId;
            }

            return episodesData;
        }

        /// <summary>
        /// Получение информации о доступных видео по одной серии
        /// </summary>
        /// <param name="episodeId"></param>
        /// <returns></returns>
        public async Task<List<VideoData>> GetVideoDatasAsync(long episodeId)
        {
            var uri = _uriFactory.GetEpisodeWatchData(episodeId);
            var request = _requestParserFactory.GetJsonRequestMessage(uri);
            var document = await _requestParserHandler.SendJsonRequestAsync(request);

            var episodeWatchData = _parserFromIDocument.GetVideoDatas(document);

            foreach (var data in episodeWatchData)
            {
                data.EpisodeId = episodeId;
            }

            return episodeWatchData;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _requestParserHandler.Dispose();
        }


    }


}
