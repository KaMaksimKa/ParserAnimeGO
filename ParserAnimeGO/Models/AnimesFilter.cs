using ParserAnimeGO.Constants;

namespace ParserAnimeGO.Models
{
    public class AnimesFilter
    {
        public required string Name { get; set; }
        public string LogicalOperator { get; set; } = AnimesFilterConstants.LogicalOperators.Or;
        public List<string> Values { get; set; } = new List<string>();
    }
}
