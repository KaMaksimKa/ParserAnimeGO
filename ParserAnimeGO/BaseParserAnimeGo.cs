using AngleSharp;
using ParserAnimeGO.AnimeData;
using ParserAnimeGO.Interface;

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
                .GetHtmlRequestMessage(_uriFactory.GetAnimesByPageUri(numberOfPage));
            using var document = await _requestParserHandler.SendHtmlRequestAsync(requestMessage);
            
            return _parserFromIDocument.GetPartialAnime(document);
        }

        public async Task<MainAnimeData> GetMainAnimeDataByAnimeHrefGoAsync(string animeHref)
        {
            using var requestMessage = _requestParserFactory.GetHtmlRequestMessage(new Uri(animeHref));
            using var document = await _requestParserHandler.SendHtmlRequestAsync(requestMessage);

            return _parserFromIDocument.GetMainDataAnime(document);
        }

        public async Task<ShowAnimeData> GetShowAnimeDataByIdFromAnimeGoAsync(int idFromAnimeGo)
        {
            using var requestMessage = _requestParserFactory.GetJsonRequestMessage(
                _uriFactory.GetShowDataAnimeByIdFromAnimeGoUri(idFromAnimeGo));
            using var document = await _requestParserHandler.SendJsonRequestAsync(requestMessage);

            return _parserFromIDocument.GetShowDataAnime(document);
        }

        public async Task<DubbingAnimeData> GetDubbingAnimeDataByIdFromAnimeGoAsync(int idFromAnimeGo)
        {
            using var requestMessage = _requestParserFactory.GetJsonRequestMessage(
                _uriFactory.GetVoiceoverDataAnimeByIdFromAnimeGoUri(idFromAnimeGo));
            using var document = await _requestParserHandler.SendJsonRequestAsync(requestMessage);

            return _parserFromIDocument.GetDubbingDataAnimeFromPlayerAsync(document);
        }
        
        public void Dispose()
        {
            _requestParserHandler.Dispose();
        }
    }
}
