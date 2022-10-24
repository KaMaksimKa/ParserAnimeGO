using ParserAnimeGO.AnimeData;
using ParserAnimeGO.Interface;

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
            
            await Parallel.ForEachAsync(animesFromParser, async (anime,_) =>
                {
                    if (anime.Href is {} animeHref 
                        && await GetMainAnimeDataByAnimeHrefGoAsync(animeHref) is { } mainAnimeData)
                    {
                        anime.UpdateWithMainAnimeData(mainAnimeData);
                    }

                    if (anime.IdFromAnimeGo is {} idFromAnimeGo)
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
                });

            return animesFromParser;
        }

        public async Task<List<PartialAnimeData>> GetPartialAnimeDataRangeAsync(List<int> idFromAnimeGoList)
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

            return partialAnimesData.Where(d => idFromAnimeGoList.Contains(d.IdFromAnimeGo)).ToList();
        }


    }
}
