using ParserAnimeGO.Interface;
using ParserAnimeGO.Models;

namespace ParserAnimeGO
{
    public class ParserAnimeGo : BaseParserAnimeGo
    {
        private const int NumberOfFirstPage = 1;

        public ParserAnimeGo(IRequestParserHandler requestParserHandler,
            IRequestParserFactory requestParserFactory,
            IAnimeParserFromIDocument parserFromIDocument,
            IAnimeGoUriFactory uriFactory) : base(requestParserHandler, requestParserFactory, parserFromIDocument, uriFactory)
        {

        }

        public async Task<List<AnimeFromParser>> GetFullAnimesFromAllPagesAsync(AnimesArgs? animesArgs = null)
        {
            List<AnimeFromParser> animesFromParser = new List<AnimeFromParser>();

            animesArgs ??= new AnimesArgs();
            int numberOfPage = NumberOfFirstPage;
            while (true)
            {
                animesArgs.PageNumber = numberOfPage;
                var animes = await GetFullAnimesByArgsAsync(animesArgs);
                if (animes.Count == 0)
                {
                    break;
                }
                animesFromParser.AddRange(animes);
                numberOfPage++;
            }


            return animesFromParser;
        }

        public async Task<List<AnimeFromParser>> GetFullAnimesFromPageRangeAsync(int start, int count, AnimesArgs? animesArgs = null)
        {
            List<AnimeFromParser> animesFromParser = new List<AnimeFromParser>();

            animesArgs ??= new AnimesArgs();
            for (int numberOfPage = start; numberOfPage < start + count; numberOfPage++)
            {
                animesArgs.PageNumber = numberOfPage;
                animesFromParser.AddRange(await GetFullAnimesByArgsAsync(animesArgs));
            }

            return animesFromParser;
        }

        public async Task<List<AnimeFromParser>> GetFullAnimesByArgsAsync(AnimesArgs animesArgs)
        {
            List<PartialAnimeData> partialAnimesData = await GetPartialAnimesDataByArgsAsync(animesArgs);

            return await GetFullAnimesFromPartialAnimesData(partialAnimesData);
        }

        public async Task<List<AnimeFromParser>> GetFullAnimesRangeAsync(List<long> idFromAnimeGoList)
        {
            List<PartialAnimeData> partialAnimesData = await GetPartialAnimeDataRangeAsync(idFromAnimeGoList);

            return await GetFullAnimesFromPartialAnimesData(partialAnimesData);
        }

        public async Task<List<PartialAnimeData>> GetPartialAnimeDataRangeAsync(List<long> idFromAnimeGoList)
        {
            List<PartialAnimeData> partialAnimesData = new List<PartialAnimeData>();

            int numberOfPage = NumberOfFirstPage;
            while (true)
            {
                var partialAnimeData = await GetPartialAnimesDataByArgsAsync(new AnimesArgs()
                {
                    PageNumber = numberOfPage
                });

                if (partialAnimeData.Count == 0)
                {
                    break;
                }
                partialAnimesData.AddRange(partialAnimeData.Where(x => x.IdFromAnimeGo.HasValue && idFromAnimeGoList.Contains(x.IdFromAnimeGo.Value)).ToList());

                if (partialAnimesData.Count == idFromAnimeGoList.Count)
                {
                    break;
                }

                numberOfPage++;
            }

            return partialAnimesData;
        }

        public async Task<List<MainAnimeData>> GetMainAnimeDataRangeAsync(List<long> idFromAnimeGoList)
        {

            var partialAnimesData = await GetPartialAnimeDataRangeAsync(idFromAnimeGoList);
            var mainAnimesData = new List<MainAnimeData>();
            foreach (var partialAnimeData in partialAnimesData)
            {
                if (partialAnimeData.Href is { } href
                    && await GetMainAnimeDataByAnimeHrefGoAsync(href) is { } mainAnimeData)
                {
                    mainAnimesData.Add(mainAnimeData);
                }
            }
            return mainAnimesData;
        }

        public async Task<List<ShowAnimeData>> GetShowAnimeDataRangeAsync(List<long> idFromAnimeGoList)
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

        public async Task<List<DubbingAnimeData>> GetDubbingAnimeDataRangeAsync(List<long> idFromAnimeGoList)
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

        public async Task<List<AnimeCommentFromParser>> GetAllCommentsFromAnime(long idForComments,int numberOfCommentsPerRequest = 20)
        {
            var comments = new List<AnimeCommentFromParser>();

            List<AnimeCommentFromParser> commentsByAnime;
            var numberOfPage = NumberOfFirstPage;
            do
            {
                commentsByAnime = await GetAnimeCommentsAsync(idForComments, numberOfPage, numberOfCommentsPerRequest);
                comments.AddRange(commentsByAnime);
                numberOfPage++;
            } while (commentsByAnime.Any());

            return comments;
        }

        public async Task<List<AnimeFromParser>> GetFullAnimesFromPartialAnimesData(List<PartialAnimeData> partialAnimesData)
        {
            List<AnimeFromParser> animesFromParser = new List<AnimeFromParser>();
            foreach (var animeData in partialAnimesData)
            {
                if (animeData.IdFromAnimeGo is { } idFromAnimeGo)
                {
                    var anime = new AnimeFromParser { IdFromAnimeGo = idFromAnimeGo };
                    anime.UpdateWithPartialAnimeData(animeData);
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
    }
}
