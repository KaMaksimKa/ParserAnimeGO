using ParserAnimeGO.Interface;
using ParserAnimeGO.Models;

namespace ParserAnimeGO
{
    public class ParserAnimeGo:BaseParserAnimeGo
    {
        private const int NumberOfFirstPage = 1;
        public ParserAnimeGo(IRequestParserHandler requestParserHandler,
            IRequestParserFactory requestParserFactory,
            IAnimeParserFromIDocument parserFromIDocument,
            IAnimeGoUriFactory uriFactory) : base(requestParserHandler, requestParserFactory, parserFromIDocument, uriFactory)
        {
            
        }

        public async Task<List<AnimeFromParser>> GetFullAnimesFromAllPagesAsync()
        {
            List<AnimeFromParser> animesFromParser = new List<AnimeFromParser>();

            int i = NumberOfFirstPage;
            while (true)
            {
                var animes = await GetFullAnimesByPageAsync(i);
                if (animes.Count == 0)
                {
                    break;
                }
                animesFromParser.AddRange(animes);
                i++;
            }
            

            return animesFromParser;
        }

        public async Task<List<AnimeFromParser>> GetFullAnimesFromPageRangeAsync(int numberOfPage,int count)
        {
            List<AnimeFromParser> animesFromParser = new List<AnimeFromParser>();

            for (int i = numberOfPage; i < numberOfPage+count; i++)
            {
                animesFromParser.AddRange(await GetFullAnimesByPageAsync(i));
            }

            return animesFromParser;
        }

        public async Task<List<AnimeFromParser>> GetFullAnimesByPageAsync(int numberOfPage)
        {
            List<PartialAnimeData> partialAnimesData = await GetPartialAnimesDataByPageAsync(numberOfPage);
            List<AnimeFromParser> animesFromParser = new List<AnimeFromParser>();
            foreach (var animeData in partialAnimesData)
            {
                if (animeData.IdFromAnimeGo is {} idFromAnimeGo)
                {
                    var anime = new AnimeFromParser { IdFromAnimeGo = idFromAnimeGo };
                    anime.UpdateWithDubbingPartialAnimeData(animeData);
                    animesFromParser.Add(anime);
                }
            }

            foreach (var anime in animesFromParser)
            {
                if (anime.Href is { } animeHref
                    && await GetMainAnimeDataByAnimeHrefGoAsync(animeHref) is { } mainAnimeData)
                {
                    anime.UpdateWithMainAnimeData(mainAnimeData);
                }

                if (anime.IdFromAnimeGo is { } idFromAnimeGo)
                {
                    if (await GetShowAnimeDataByIdFromAnimeGoAsync(idFromAnimeGo) is { } showAnimeData)
                    {
                        anime.UpdateWithShowAnimeData(showAnimeData);
                    }

                    if (await GetDubbingAnimeDataByIdFromAnimeGoAsync(idFromAnimeGo) is { } dubbingAnimeData)
                    {
                        anime.UpdateWithDubbingAnimeData(dubbingAnimeData);
                    }
                }
            }


            return animesFromParser;
        }

        public async Task<List<PartialAnimeData>> GetPartialAnimeDataRangeAsync(List<long> idFromAnimeGoList)
        {
            List<PartialAnimeData> partialAnimesData = new List<PartialAnimeData>();

            int i = NumberOfFirstPage;
            while (true)
            {
                var partialAnimeData = await GetPartialAnimesDataByPageAsync(i);
                if (partialAnimeData.Count == 0)
                {
                    break;
                }
                partialAnimesData.AddRange(partialAnimeData);
                i++;
            }

            return partialAnimesData.Where(x =>x.IdFromAnimeGo.HasValue && idFromAnimeGoList.Contains(x.IdFromAnimeGo.Value)).ToList();
        }

        public async Task<List<MainAnimeData>> GetMainAnimeDataRangeAsync(List<long> idFromAnimeGoList)
        {
            var partialAnimesData = await GetPartialAnimeDataRangeAsync(idFromAnimeGoList);

            var hrefList = new List<string>();
            foreach (var partialAnimeData in partialAnimesData)
            {
                if (partialAnimeData.Href is { } href)
                {
                    hrefList.Add(href);
                }
            }
            return await GetMainAnimeDataRangeAsync(hrefList);
        }

        public async Task<List<ShowAnimeData>> GetShowAnimeDataRangeAsync(List<int> idFromAnimeGoList)
        {
            List<ShowAnimeData> showAnimesData = new List<ShowAnimeData>();

            foreach (var idFromAnimeGo in idFromAnimeGoList)
            {
                if (await GetShowAnimeDataByIdFromAnimeGoAsync(idFromAnimeGo) is { } showAnimeData)
                {
                    showAnimesData.Add(showAnimeData);
                }
            }

            return showAnimesData;
        }

        public async Task<List<DubbingAnimeData>> GetDubbingAnimeDataRangeAsync(List<int> idFromAnimeGoList)
        {
            List<DubbingAnimeData> dubbingAnimesData = new List<DubbingAnimeData>();

            foreach (var idFromAnimeGo in idFromAnimeGoList)
            {
                if (await GetDubbingAnimeDataByIdFromAnimeGoAsync(idFromAnimeGo) is { } dubbingAnimeData)
                {
                    dubbingAnimesData.Add(dubbingAnimeData);
                }
            }

            return dubbingAnimesData;
        }

        private async Task<List<MainAnimeData>> GetMainAnimeDataRangeAsync(List<string> hrefList)
        {
            List<MainAnimeData> mainAnimesData = new List<MainAnimeData>();

            foreach (var href in hrefList)
            {
                if (await GetMainAnimeDataByAnimeHrefGoAsync(href) is { } mainAnimeData)
                {
                    mainAnimesData.Add(mainAnimeData);
                }
            }

            return mainAnimesData;
        }
    }
}
