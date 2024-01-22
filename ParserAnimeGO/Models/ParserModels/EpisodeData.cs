using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserAnimeGO.Models.ParserModels
{
    public class EpisodeData
    {
        public long AnimeId { get; set; }
        public long? EpisodeId { get; set; }
        public int? EpisodeNumber {  get; set; }
        public string? EpisodeTitle { get; set; }
        public DateTimeOffset? EpisodeReleased { get; set; }
        public int? EpisodeType { get; set; }
        public string? EpisodeDescription { get; set; }
    }
}
