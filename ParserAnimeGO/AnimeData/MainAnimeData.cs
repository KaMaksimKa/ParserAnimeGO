namespace ParserAnimeGO.AnimeData
{
    public class MainAnimeData
    {
        public int? IdFromAnimeGo { get; set; }
        public string? TitleEn { get; set; }
        public string? TitleRu { get; set; }
        public string? Description { get; set; }
        public double? Rate { get; set; }
        public string? NextEpisode { get; set; }
        public string? Type { get; set; }
        public int? CountEpisode { get; set; }
        public string? Status { get; set; }
        public List<string> Genres { get; set; } = new List<string>();
        public string? OriginalSource { get; set; }
        public string? Season { get; set; }
        public string? Release { get; set; }
        public List<string> Studios { get; set; } = new List<string>();
        public string? MpaaRate { get; set; }
        public string? AgeRestrictions { get; set; }
        public string? Duration { get; set; }
        public List<string> Dubbing { get; set; } = new List<string>();
        public string? ShotByRanobe { get; set; }
        public string? ImgHref { get; set; }
    }
}
