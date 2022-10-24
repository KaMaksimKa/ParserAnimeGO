using ParserAnimeGO.AnimeData;

namespace ParserAnimeGO.ConsoleApp.Data
{
    internal interface IAnimeRecordingRepository
    {
        Task AddOrUpdateRangeAsync(List<AnimeFromParser> animes);
        Task AddOrUpdateAsync(AnimeFromParser anime);
        Task AddRangeAsync(List<AnimeFromParser> animes);
        Task AddAsync(AnimeFromParser anime);
        Task UpdateRangeAsync(List<AnimeFromParser> animes);
        Task UpdateAsync(AnimeFromParser animes);
        Task UpdateWithPartialAnimeDataAsync(PartialAnimeData animeData);
        Task UpdateWithMainDataAsync(MainAnimeData animeData);
        Task UpdateWithShowDataAsync(ShowAnimeData animeData);
        Task UpdateWithDubbingDataAsync(DubbingAnimeData animeData);
    }
}
