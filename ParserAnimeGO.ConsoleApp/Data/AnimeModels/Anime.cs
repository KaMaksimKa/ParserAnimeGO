using ParserAnimeGO.AnimeData;

namespace ParserAnimeGO.ConsoleApp.Data.AnimeModels
{
    public class Anime
    {
        public int AnimeId { get; set; }
        public string? TitleEn { get; set; }
        public string? TitleRu { get; set; }
        public int? Year { get; set; }
        public string? Description { get; set; }
        public double? Rate { get; set; }
        public int? CountEpisode { get; set; }
        public int? Planned { get; set; }
        public int? Completed { get; set; }
        public int? Watching { get; set; }
        public int? Dropped { get; set; }
        public int? OnHold { get; set; }
        public string? Href { get; set; }
        public string? ImgHref { get; set; }
        public string? NextEpisode { get; set; }
        public int IdFromAnimeGo { get; set; }
        public string? Duration { get; set; }
        public int? TypeId { get; set; }
        public TypeAnime? Type { get; set; }
        public int? StatusId { get; set; }
        public Status? Status { get; set; }
        public int? MpaaRateId { get; set; }
        public MpaaRate? MpaaRate { get; set; }
        public List<Studio> Studios { get; set; } = new List<Studio>();
        public List<Dubbing> Dubbings { get; set; } = new List<Dubbing>();
        public List<Genre> Genres { get; set; } = new List<Genre>();

    }
}

