using ParserAnimeGO.Interface;
using ParserAnimeGO.Models;

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
        /// <param name="idFromAnimeGo"></param>
        /// <returns></returns>
        public async Task<ShowAnimeData?> GetShowAnimeDataByIdFromAnimeGoAsync(long idFromAnimeGo)
        {
            using var requestMessage = _requestParserFactory.GetJsonRequestMessage(
                _uriFactory.GetShowDataAnime(idFromAnimeGo));
            using var document = await _requestParserHandler.SendJsonRequestAsync(requestMessage);

            var showAnimeData = _parserFromIDocument.GetShowDataAnime(document);

            if (showAnimeData != null)
            {
                showAnimeData.IdFromAnimeGo = idFromAnimeGo;
            }


            return showAnimeData;
        }

        /// <summary>
        /// Получить информацию о озвучке на первой серии отдельного аниме
        /// </summary>
        /// <param name="idFromAnimeGo"></param>
        /// <returns></returns>
        public async Task<DubbingAnimeData?> GetDubbingAnimeDataByIdFromAnimeGoAsync(long idFromAnimeGo)
        {
            using var requestMessage = _requestParserFactory.GetJsonRequestMessage(
                _uriFactory.GetVoiceoverDataAnime(idFromAnimeGo));
            using var document = await _requestParserHandler.SendJsonRequestAsync(requestMessage);

            var dubbingAnimeData = _parserFromIDocument.GetDubbingDataAnimeFromPlayerAsync(document);

            if (dubbingAnimeData != null)
            {
                dubbingAnimeData.IdFromAnimeGo = idFromAnimeGo;
            }

            return dubbingAnimeData;
        }

        /// <summary>
        /// Получить информацию об выходе новых серий
        /// </summary>
        /// <returns></returns>
        public async Task<List<AnimeNotificationFromParser>> GetAnimeNotificationsFromAnimeGoAsync()
        {
            var uri = _uriFactory.GetAnimeNotifications();
            var request = _requestParserFactory.GetHtmlRequestMessage(uri);
            var document = await _requestParserHandler.SendHtmlRequestAsync(request);

            return _parserFromIDocument.GetAnimeNotificationsFromParserAsync(document);
        }

        /// <summary>
        /// Получить комментарии по аниме
        /// </summary>
        /// <param name="idForComments"></param>
        /// <param name="numberOfPage"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<AnimeCommentFromParser>> GetAnimeCommentsAsync(long idForComments, int numberOfPage = 1, int limit = 20)
        {
            var uri = _uriFactory.GetAnimeComments(idForComments, numberOfPage,limit);
            var request = _requestParserFactory.GetJsonRequestMessage(uri);
            var document = await _requestParserHandler.SendJsonRequestAsync(request);

            return _parserFromIDocument.GetAnimeComments(document);
        }

        /// <summary>
        /// Получить фото по аниме
        /// </summary>
        /// <param name="imgIdFromAnimeGo"></param>
        /// <returns></returns>
        public async Task<Stream> GetAnimeImageAsync(string imgIdFromAnimeGo)
        {
            var uri = _uriFactory.GetAnimeImage(imgIdFromAnimeGo);
            var request = _requestParserFactory.GetImageRequestMessage(uri);
            return await _requestParserHandler.SendImageRequestAsync(request);
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
