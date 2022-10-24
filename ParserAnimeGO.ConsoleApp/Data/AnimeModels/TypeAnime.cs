namespace ParserAnimeGO.ConsoleApp.Data.AnimeModels
{
    public class TypeAnime : IHavingTitle
    {
        public int TypeAnimeId { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; } = string.Empty;
        public List<Anime> Animes { get; set; }
    }
}
