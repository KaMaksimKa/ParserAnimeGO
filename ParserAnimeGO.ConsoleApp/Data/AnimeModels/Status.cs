namespace ParserAnimeGO.ConsoleApp.Data.AnimeModels
{
    public class Status : IHavingTitleAndFriendlyUrl
    {
        public int StatusId { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; } = string.Empty;
        public List<Anime> Animes { get; set; }
        public static Status FromString(string title) => new Status() { Title = title };
    }
}
