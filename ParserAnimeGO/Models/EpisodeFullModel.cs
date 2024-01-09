using ParserAnimeGO.Models.ParserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserAnimeGO.Models
{
    public class EpisodeFullModel
    {
        public EpisodeData EpisodeData { get; set; } = null!;
        public List<EpisodeWatchData> EpisodeWatchData { get; set; } = null!;
    }
}
