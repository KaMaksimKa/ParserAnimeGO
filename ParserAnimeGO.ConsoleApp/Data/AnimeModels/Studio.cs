namespace ParserAnimeGO.ConsoleApp.Data.AnimeModels
{
    public class Studio : IHavingTitle
    {
        public int StudioId { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; } = string.Empty;
        public List<Anime> Animes { get; set; }
    }
}
