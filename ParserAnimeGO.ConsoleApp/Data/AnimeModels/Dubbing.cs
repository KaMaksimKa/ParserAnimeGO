namespace ParserAnimeGO.ConsoleApp.Data.AnimeModels
{
    public class Dubbing : IHavingTitle
    {
        public int DubbingId { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; } = string.Empty;
        public List<Anime> Animes { get; set; }

    }
}
