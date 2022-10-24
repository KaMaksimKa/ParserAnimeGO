namespace ParserAnimeGO.ConsoleApp.Data.AnimeModels
{
    public class Genre : IHavingTitle
    {
        public int GenreId { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; } = string.Empty;
        public List<Anime> Animes { get; set; }
    }
}
