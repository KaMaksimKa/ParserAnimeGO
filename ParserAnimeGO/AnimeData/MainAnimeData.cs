namespace ParserAnimeGO.AnimeData
{
    public class MainAnimeData
    {
        public int IdFromAnimeGo { get; set; }
        public double? Rate { get; set; }
        public string? Status { get; set; }
        public int? CountEpisode { get; set; }
        public string? MpaaRate { get; set; }
        public string? ImgHref { get; set; }
        public string? NextEpisode { get; set; }
        public string? Duration { get; set; }
        public List<string> Studios { get; set; } = new List<string>();
        public List<string> Genres { get; set; } = new List<string>();
        public List<string> Dubbing { get; set; } = new List<string>();
    }
}
