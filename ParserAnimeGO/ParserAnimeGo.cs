using ParserAnimeGO.Interface;
using ParserAnimeGO.Models;
using ParserAnimeGO.Models.ParserModels;

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

        public async Task<List<AnimeFullModel>> GetFullAnimesFromAllPagesAsync(AnimesArgs? animesArgs = null)
        {
            List<AnimeFullModel> animesFromParser = new List<AnimeFullModel>();

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

        public async Task<List<AnimeFullModel>> GetFullAnimesFromRangePageAsync(int start, int count, AnimesArgs? animesArgs = null)
        {
            List<AnimeFullModel> animesFromParser = new List<AnimeFullModel>();

            animesArgs ??= new AnimesArgs();
            for (int numberOfPage = start; numberOfPage < start + count; numberOfPage++)
            {
                animesArgs.PageNumber = numberOfPage;
                animesFromParser.AddRange(await GetFullAnimesByArgsAsync(animesArgs));
            }

            return animesFromParser;
        }

        public async Task<List<AnimeFullModel>> GetFullAnimesByArgsAsync(AnimesArgs animesArgs)
        {
            List<PartialAnimeData> partialAnimesData = await GetPartialAnimesDataByArgsAsync(animesArgs);

            return await GetFullAnimesFromPartialAnimesData(partialAnimesData);
        }

        public async Task<List<AnimeFullModel>> GetFullAnimesRangeAsync(List<long> idFromAnimeGoList)
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
                partialAnimesData.AddRange(partialAnimeData.Where(x => x.AnimeId.HasValue && idFromAnimeGoList.Contains(x.AnimeId.Value)).ToList());

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

            foreach (var animeId in idFromAnimeGoList)
            {
                if (await GetShowAnimeDataByIdFromAnimeGoAsync(animeId) is { } showAnimeData)
                {
                    showAnimesData.Add(showAnimeData);
                }
            }

            return showAnimesData;
        }

        public async Task<List<AnimeCommentData>> GetAllCommentsFromAnime(long idForComments,int numberOfCommentsPerRequest = 20)
        {
            var comments = new List<AnimeCommentData>();

            List<AnimeCommentData> commentsByAnime;
            var numberOfPage = NumberOfFirstPage;
            do
            {
                commentsByAnime = await GetAnimeCommentsAsync(idForComments, numberOfPage, numberOfCommentsPerRequest);
                comments.AddRange(commentsByAnime);
                numberOfPage++;
            } while (commentsByAnime.Any());

            return comments;
        }

        public async Task<List<AnimeFullModel>> GetFullAnimesFromPartialAnimesData(List<PartialAnimeData> partialAnimesData)
        {
            List<AnimeFullModel> animesFromParser = new List<AnimeFullModel>();
            foreach (var animeData in partialAnimesData)
            {
                if (animeData.AnimeId is { } animeId)
                {
                    var anime = new AnimeFullModel { AnimeId = animeId };
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

                if (anime.AnimeId is { } animeId)
                {
                    if (await GetShowAnimeDataByIdFromAnimeGoAsync(animeId) is { } showAnimeData)
                    {
                        anime.UpdateWithShowAnimeData(showAnimeData);
                    }
                }
            }

            return animesFromParser;
        }

        public async Task<List<EpisodeFullModel>> GetEpisodeFullModel(long animeId)
        {
            var episodeFullModels = new List<EpisodeFullModel>();

            var episodesData = await GetEpisodesDataAsync(animeId);
            foreach (var episode in episodesData)
            {
                if (episode.EpisodeId.HasValue)
                {
                    var episodeWatchData = await GetVideoDatasAsync(episode.EpisodeId.Value);
                    episodeFullModels.Add(new EpisodeFullModel()
                    {
                        EpisodeData = episode,
                        VideoDatas = episodeWatchData,
                    });
                }
            }

            return episodeFullModels;
        }
    }
}
