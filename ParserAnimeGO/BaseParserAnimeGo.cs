using ParserAnimeGO.Interface;
using ParserAnimeGO.Models;

namespace ParserAnimeGO
{
    public class BaseParserAnimeGo:IDisposable
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

        public async Task<List<PartialAnimeData>> GetPartialAnimesDataByPageAsync(int numberOfPage)
        {
            using var requestMessage = _requestParserFactory
                .GetHtmlRequestMessage(_uriFactory.GetAnimes(new AnimesArgs()
                {
                    PageNumber = numberOfPage
                }));
            using var document = await _requestParserHandler.SendHtmlRequestAsync(requestMessage);
            
            return _parserFromIDocument.GetPartialAnime(document);
        }

        public async Task<MainAnimeData?> GetMainAnimeDataByAnimeHrefGoAsync(string animeHref)
        {
            using var requestMessage = _requestParserFactory.GetHtmlRequestMessage(new Uri(animeHref));
            using var document = await _requestParserHandler.SendHtmlRequestAsync(requestMessage);

            return _parserFromIDocument.GetMainDataAnime(document);
        }

        public async Task<ShowAnimeData?> GetShowAnimeDataByIdFromAnimeGoAsync(int idFromAnimeGo)
        {
            using var requestMessage = _requestParserFactory.GetJsonRequestMessage(
                _uriFactory.GetShowDataAnimeById(idFromAnimeGo));
            using var document = await _requestParserHandler.SendJsonRequestAsync(requestMessage);

            var showAnimeData = _parserFromIDocument.GetShowDataAnime(document);

            if (showAnimeData != null)
            {
                showAnimeData.IdFromAnimeGo = idFromAnimeGo;
            }
            

            return showAnimeData;
        }

        public async Task<DubbingAnimeData?> GetDubbingAnimeDataByIdFromAnimeGoAsync(int idFromAnimeGo)
        {
            using var requestMessage = _requestParserFactory.GetJsonRequestMessage(
                _uriFactory.GetVoiceoverDataAnimeById(idFromAnimeGo));
            using var document = await _requestParserHandler.SendJsonRequestAsync(requestMessage);

            var dubbingAnimeData = _parserFromIDocument.GetDubbingDataAnimeFromPlayerAsync(document);

            if (dubbingAnimeData != null)
            {
                dubbingAnimeData.IdFromAnimeGo = idFromAnimeGo;
            }
            
            return dubbingAnimeData;
        }

        public async Task<List<AnimeNotificationFromParser>> GetAnimeNotificationsFromAnimeGoAsync()
        {
            var uri = _uriFactory.GetAnimeNotifications();
            var request = _requestParserFactory.GetHtmlRequestMessage(uri);
            var document = await _requestParserHandler.SendHtmlRequestAsync(request);

            return _parserFromIDocument.GetAnimeNotificationsFromParserAsync(document);
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
