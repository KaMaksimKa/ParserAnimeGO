namespace ParserAnimeGO.ConsoleApp.Data.AnimeModels
{
    public class Dubbing : IHavingTitleAndFriendlyUrl
    {
        public int DubbingId { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; } = string.Empty;
        public List<Anime> Animes { get; set; }

        public static Dubbing FromString(string title) => new Dubbing() {Title = title };
    }
}
