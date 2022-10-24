﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ParserAnimeGO.AnimeData
{
    public class AnimeFromParser
    {
        public int IdFromAnimeGo { get; set; }
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

        public void UpdateWithDubbingPartialAnimeData(PartialAnimeData partialAnimeData)
        {
            TitleEn = partialAnimeData.TitleEn;
            Type = partialAnimeData.Type;
            Year = partialAnimeData.Year;
            Description = partialAnimeData.Description;
            Href = partialAnimeData.Href;
        }
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
            Dubbing = mainAnimeData.Dubbing.ToList();

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


        public PartialAnimeData ToPartialAnimeData() => new PartialAnimeData
        {
            IdFromAnimeGo = IdFromAnimeGo,
            TitleEn = TitleEn,
            Type = Type,
            Year = Year,
            Description = Description,
            Href = Href,
        };
        public MainAnimeData ToMainAnimeData() => new MainAnimeData
        {
            Rate = Rate,
            Status = Status,
            CountEpisode = CountEpisode,
            MpaaRate = MpaaRate,
            ImgHref = ImgHref,
            NextEpisode = NextEpisode,
            Studios = Studios,
            Genres = Genres,
            Dubbing = Dubbing,
        };
        public ShowAnimeData ToShowAnimeData() => new ShowAnimeData
        {
            Planned = Planned,
            Completed = Completed,
            Watching = Watching,
            Dropped = Dropped,
            OnHold = OnHold,
        };
        public DubbingAnimeData ToDubbingAnimeData() => new DubbingAnimeData
        {
            Dubbing = Dubbing
        };
   
    }
}
