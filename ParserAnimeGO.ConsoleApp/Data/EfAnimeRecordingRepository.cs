using Microsoft.EntityFrameworkCore;
using ParserAnimeGO.AnimeData;
using ParserAnimeGO.ConsoleApp.Data.AnimeModels;

namespace ParserAnimeGO.ConsoleApp.Data
{
    internal class EfAnimeRecordingRepository : IAnimeRecordingRepository
    {
        private readonly ApplicationContext _context;

        
        public EfAnimeRecordingRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddOrUpdateRangeAsync(List<AnimeFromParser> animesFromParsers)
        {
            var animesAdded = new List<AnimeFromParser>();
            var animesUpdated = new List<AnimeFromParser>();
            foreach (var animeFromParser in animesFromParsers)
            {
                if (await GetByIdFromAnimeGoOrDefault(animeFromParser.IdFromAnimeGo) is { })
                {
                    animesUpdated.Add(animeFromParser);
                }
                else
                {
                    animesAdded.Add(animeFromParser);
                }
            }

            await UpdateRangeAsync(animesUpdated);
            await AddRangeAsync(animesAdded);
        }

        public async Task AddOrUpdateAsync(AnimeFromParser anime)
        {
            await AddOrUpdateRangeAsync(new List<AnimeFromParser> { anime });
        }

        public async Task AddRangeAsync(List<AnimeFromParser> animesFromParsers)
        {
            var animes = new List<Anime>();
            foreach (var animeFromParser in animesFromParsers)
            {
                var anime = new Anime{IdFromAnimeGo = animeFromParser.IdFromAnimeGo};
                await UpdateWithAnimeFromParser(animeFromParser, anime);
                animes.Add(anime);
            }

            await _context.Animes.AddRangeAsync(animes);
        }

        public async Task AddAsync(AnimeFromParser anime)
        {
            await AddRangeAsync(new List<AnimeFromParser> { anime });
        }

        public async Task UpdateRangeAsync(List<AnimeFromParser> animes)
        {
            foreach (var anime in animes)
            {
                await UpdateAsync(anime);
            }
        }
        
        public async Task UpdateAsync(AnimeFromParser animes)
        {
            if (await GetByIdFromAnimeGoOrDefault(animes.IdFromAnimeGo) is { } anime)
            { 
                await UpdateWithAnimeFromParser(animes, anime);
            }
        }
        
        public async Task UpdateWithPartialAnimeDataAsync(PartialAnimeData animeData)
        {
            if (await GetByIdFromAnimeGoOrDefault(animeData.IdFromAnimeGo) is { } anime)
            {
                await UpdateWithPartialAnimeData(animeData, anime);
            }
        }

        public async Task UpdateWithMainDataAsync(MainAnimeData animeData)
        {
            if (await GetByIdFromAnimeGoOrDefault(animeData.IdFromAnimeGo) is { } anime)
            {
                await UpdateWithMainData(animeData, anime);
            }
        }

        public async Task UpdateWithShowDataAsync(ShowAnimeData animeData)
        {
            if (await GetByIdFromAnimeGoOrDefault(animeData.IdFromAnimeGo) is { } anime)
            {
                await UpdateWithShowData(animeData, anime);
            }
        }

        public async Task UpdateWithDubbingDataAsync(DubbingAnimeData animeData)
        {
            if (await GetByIdFromAnimeGoOrDefault(animeData.IdFromAnimeGo) is { } anime)
            {
                await UpdateWithDubbingData(animeData, anime);
            }
        }

        private async Task UpdateWithAnimeFromParser(AnimeFromParser animeData, Anime anime)
        {
            await UpdateWithPartialAnimeData(animeData.ToPartialAnimeData(), anime);
            await UpdateWithMainData(animeData.ToMainAnimeData(), anime);
            await UpdateWithShowData(animeData.ToShowAnimeData(), anime);
            await UpdateWithDubbingData(animeData.ToDubbingAnimeData(), anime);
        }

        private async Task UpdateWithPartialAnimeData(PartialAnimeData animeData, Anime anime)
        {
            anime.Year = animeData.Year;
            anime.Description = animeData.Description;
            anime.TitleRu = animeData.TitleRu;
            anime.TitleEn = animeData.TitleEn;
            anime.Href = animeData.Href;
            anime.Type = animeData.Type == null ? null : await AddIHaveTitleByString(animeData.Type, _context.Types);
        }

        private async Task UpdateWithMainData(MainAnimeData animeData, Anime anime)
        {

            anime.Rate = animeData.Rate;
            anime.Duration = animeData.Duration;
            anime.CountEpisode = animeData.CountEpisode;
            anime.NextEpisode = animeData.NextEpisode;
            anime.ImgHref = animeData.ImgHref;
            anime.Status = animeData.Status == null ? null : await AddIHaveTitleByString(animeData.Status, _context.Statuses);
            anime.MpaaRate = animeData.MpaaRate == null ? null : await AddIHaveTitleByString(animeData.MpaaRate, _context.MpaaRates);
            anime.Studios = await AddRangeIHaveTitleByString(animeData.Studios, _context.Studios);
            anime.Genres = await AddRangeIHaveTitleByString(animeData.Genres, _context.Genres);
            anime.Dubbings = await AddRangeIHaveTitleByString(animeData.Dubbing, _context.Dubbing);

        }

        private Task UpdateWithShowData(ShowAnimeData animeData, Anime anime)
        {
            anime.Completed = animeData.Completed;
            anime.Dropped = animeData.Dropped;
            anime.OnHold = animeData.OnHold;
            anime.Planned = animeData.Planned;
            anime.Watching = animeData.Watching;

            return Task.CompletedTask;
        }

        private async Task UpdateWithDubbingData(DubbingAnimeData animeData, Anime anime)
        {
            anime.Dubbings = anime.Dubbings.Union(await AddRangeIHaveTitleByString(animeData.Dubbing, _context.Dubbing)).ToList();
        }

        private async Task<List<T>> AddRangeIHaveTitleByString<T>(List<string> titleList, DbSet<T> dbSet) where T : class, IHavingTitle, new()
        {
            List<T> objList = new List<T>();
            foreach (var title in titleList)
            {
                objList.Add(await AddIHaveTitleByString(title, dbSet));
            }

            return objList;
        }

        private async Task<T> AddIHaveTitleByString<T>(string title, DbSet<T> dbSet) where T : class, IHavingTitle, new()
        {
            if (await dbSet.FirstOrDefaultAsync(o => o.Title == title) is { } objDb)
            {
                return objDb;
            }

            if (dbSet.Local.FirstOrDefault(o => o.Title == title) is { } objLocal)
            {
                return objLocal;
            }

            var obj = new T { Title = title };

            dbSet.Add(obj);

            return obj;
        }

        private async Task<Anime?> GetByIdFromAnimeGoOrDefault(int idFromAnimeGo)
        {
            if (await _context.Animes.FirstOrDefaultAsync(a => a.IdFromAnimeGo == idFromAnimeGo) is { } anime)
            {
                return anime;
            }

            return null;
        }
    }
}
