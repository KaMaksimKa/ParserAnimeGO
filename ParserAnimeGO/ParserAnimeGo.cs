using AngleSharp;
using ParserAnimeGO.AnimeData;
using ParserAnimeGO.Interface;

namespace ParserAnimeGO
{
    public class ParserAnimeGo:BaseParserAnimeGo
    {
       
        public ParserAnimeGo(IRequestParserHandler requestParserHandler,
            IRequestParserFactory requestParserFactory,
            IAnimeParserFromIDocument parserFromIDocument,
            IAnimeGoUriFactory uriFactory) : base(requestParserHandler, requestParserFactory, parserFromIDocument, uriFactory)
        {
            
        }
        public async Task<List<AnimeFromParser>> GetFullAnimesByPageAsync(int numberOfPage)
        {
            List<PartialAnimeData> partialAnimesData = await GetPartialAnimesDataByPageAsync(numberOfPage);

            List<AnimeFromParser> animesFromParser =
                partialAnimesData.Select(AnimeFromParser.FromPartialAnimeData).ToList();


            await Parallel.ForEachAsync(animesFromParser, async (anime,_) =>
                {
                    if (anime.Href is {} animeHref)
                    {
                        anime.UpdateWithMainAnimeData(await GetMainAnimeDataByAnimeHrefGoAsync(animeHref));
                    }

                    if (anime.IdFromAnimeGo is {} idFromAnimeGo)
                    {
                        anime.UpdateWithShowAnimeData(await GetShowAnimeDataByIdFromAnimeGoAsync(idFromAnimeGo));
                        anime.UpdateWithDubbingAnimeData(await GetDubbingAnimeDataByIdFromAnimeGoAsync(idFromAnimeGo));
                    }
                });

            return animesFromParser;
        }

    }
}
