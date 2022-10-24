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
        Task UpdateWithPartialAnimeDataRangeAsync(List<PartialAnimeData> animeDataList);
        Task UpdateWithPartialAnimeDataAsync(PartialAnimeData animeData);
        Task UpdateWithMainDataRangeAsync(List<MainAnimeData> animeDataList);
        Task UpdateWithMainDataAsync(MainAnimeData animeData);
        Task UpdateWithShowDataRangeAsync(List<ShowAnimeData> animeDataList);
        Task UpdateWithShowDataAsync(ShowAnimeData animeData);
        Task UpdateWithDubbingDataRangeAsync(List<DubbingAnimeData> animeDataList);
        Task UpdateWithDubbingDataAsync(DubbingAnimeData animeData);
    }
}
