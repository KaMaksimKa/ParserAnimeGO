namespace ParserAnimeGO.Models
{
    public class AnimesArgs
    {
        public int? PageNumber { get; set; }
        public string? Sort { get; set; }
        public string? Direction { get; set; }
        public string? YearFrom { get; set; }
        public string? YearTo { get; set; }
        public List<AnimesFilter> Filters { get; set; } = new List<AnimesFilter>();
    }

    
}
