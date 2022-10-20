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
            return _parserFromIDocument
                .GetPartialAnime(await _requestParserHandler
                    .SendHtmlRequestAsync(_requestParserFactory
                        .GetHtmlRequestMessage(_uriFactory
                            .GetAnimesByPageUri(numberOfPage))));
        }

        public async Task<MainAnimeData> GetMainAnimeDataByAnimeHrefGoAsync(string animeHref)
        {
            return _parserFromIDocument
                .GetMainDataAnime(await _requestParserHandler
                    .SendHtmlRequestAsync(_requestParserFactory
                        .GetHtmlRequestMessage(new Uri(animeHref))));
        }

        public async Task<ShowAnimeData> GetShowAnimeDataByIdFromAnimeGoAsync(int idFromAnimeGo)
        {
            return _parserFromIDocument
                .GetShowDataAnime(await _requestParserHandler
                    .SendJsonRequestAsync(_requestParserFactory
                        .GetJsonRequestMessage(_uriFactory
                            .GetShowDataAnimeByIdFromAnimeGoUri(idFromAnimeGo))));
        }

        public async Task<DubbingAnimeData> GetDubbingAnimeDataByIdFromAnimeGoAsync(int idFromAnimeGo)
        {
            return _parserFromIDocument
                .GetDubbingDataAnimeFromPlayerAsync(await _requestParserHandler
                    .SendJsonRequestAsync(_requestParserFactory
                        .GetJsonRequestMessage(_uriFactory
                            .GetVoiceoverDataAnimeByIdFromAnimeGoUri(idFromAnimeGo))));
        }
        
        public void Dispose()
        {
            _requestParserHandler.Dispose();
        }
    }
}
