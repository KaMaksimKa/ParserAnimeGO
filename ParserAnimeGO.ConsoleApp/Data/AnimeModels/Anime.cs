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

        public static Anime FromAnimeFromParser(AnimeFromParser animeFromParser) =>
            new Anime
            {
                Href = animeFromParser.Href,
                IdFromAnimeGo = animeFromParser.IdFromAnimeGo,
                Duration = animeFromParser.Duration,
                TitleEn = animeFromParser.TitleEn,
                Dubbings = animeFromParser.Dubbing.Select(Dubbing.FromString).ToList(),
                Completed = animeFromParser.Completed,
                CountEpisode = animeFromParser.CountEpisode,
                Description = animeFromParser.Description,
                Dropped = animeFromParser.Dropped,
                Genres = animeFromParser.Genres.Select(Genre.FromString).ToList(),
                MpaaRate = animeFromParser.MpaaRate ==null?null:MpaaRate.FromString(animeFromParser.MpaaRate),
                NextEpisode = animeFromParser.NextEpisode,
                OnHold = animeFromParser.OnHold,
                Planned = animeFromParser.Planned,
                Rate = animeFromParser.Rate,
                Status = animeFromParser.Status == null ? null : Status.FromString(animeFromParser.Status),
                Studios = animeFromParser.Studios.Select(Studio.FromString).ToList(),
                TitleRu = animeFromParser.TitleRu,
                Type = animeFromParser.Type == null ? null : TypeAnime.FromString(animeFromParser.Type),
                Watching = animeFromParser.Watching,
                Year = animeFromParser.Year
            };
    }
}

