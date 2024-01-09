namespace ParserAnimeGO.Models.ParserModels
{
    public class ShowAnimeData
    {
        public long AnimeId { get; set; }
        public int? Planned { get; set; }
        public int? Completed { get; set; }
        public int? Watching { get; set; }
        public int? Dropped { get; set; }
        public int? OnHold { get; set; }
    }
}
