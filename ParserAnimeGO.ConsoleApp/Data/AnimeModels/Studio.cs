namespace ParserAnimeGO.ConsoleApp.Data.AnimeModels
{
    public class Studio : IHavingTitleAndFriendlyUrl
    {
        public int StudioId { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; } = string.Empty;
        public List<Anime> Animes { get; set; }
        public static Studio FromString(string title) => new Studio() { Title = title };
    }
}
