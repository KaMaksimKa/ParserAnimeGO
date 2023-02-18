namespace ParserAnimeGO.ConsoleApp.Data.AnimeModels
{
    public class TypeAnime : IHavingTitleAndFriendlyUrl
    {
        public int TypeAnimeId { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; } = string.Empty;
        public List<Anime> Animes { get; set; }
        public static TypeAnime FromString(string title) => new TypeAnime() { Title = title };
    }
}
