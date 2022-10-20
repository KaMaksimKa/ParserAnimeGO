using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ParserAnimeGO.AnimeData
{
    public class AnimeFromParser
    {
        public int? IdFromAnimeGo { get; set; }
        public string? TitleEn { get; set; }
        public string? TitleRu { get; set; }
        public string? Type { get; set; }
        public int? Year { get; set; }
        public string? Description { get; set; }
        public double? Rate { get; set; }
        public string? Status { get; set; }
        public int? CountEpisode { get; set; }
        public string? MpaaRate { get; set; }
        public int? Planned { get; set; }
        public int? Completed { get; set; }
        public int? Watching { get; set; }
        public int? Dropped { get; set; }
        public int? OnHold { get; set; }
        public string? Href { get; set; }
        public string? ImgHref { get; set; }
        public string? NextEpisode { get; set; }
        public string? Duration { get; set; }
        public List<string> Studios { get; set; } = new List<string>();
        public List<string> Genres { get; set; } = new List<string>();
        public List<string> Dubbing { get; set; } = new List<string>();

        public void UpdateWithMainAnimeData(MainAnimeData mainAnimeData)
        {
            Rate = mainAnimeData.Rate;
            Status = mainAnimeData.Status;
            CountEpisode = mainAnimeData.CountEpisode;
            MpaaRate = mainAnimeData.MpaaRate;
            ImgHref = mainAnimeData.ImgHref;
            NextEpisode = mainAnimeData.NextEpisode;
            Studios = mainAnimeData.Studios;
            Genres = mainAnimeData.Genres;
            Dubbing = Dubbing.Union(mainAnimeData.Dubbing).ToList();

        }
        public void UpdateWithShowAnimeData(ShowAnimeData showAnimeData)
        {
            Planned = showAnimeData.Planned;
            Completed = showAnimeData.Completed;
            Watching = showAnimeData.Watching;
            Dropped = showAnimeData.Dropped;
            OnHold = showAnimeData.OnHold;
        }
        public void UpdateWithDubbingAnimeData(DubbingAnimeData dubbingAnimeData)
        {
            Dubbing = Dubbing.Union(dubbingAnimeData.Dubbing).ToList();
        }

        public static AnimeFromParser FromPartialAnimeData(PartialAnimeData partialAnimeData) =>
            new AnimeFromParser
            {
                IdFromAnimeGo = partialAnimeData.IdFromAnimeGo,
                TitleEn = partialAnimeData.TitleEn,
                Type = partialAnimeData.Type,
                Year = partialAnimeData.Year,
                Description = partialAnimeData.Description,
                Href = partialAnimeData.Href,
            };
    }
}
