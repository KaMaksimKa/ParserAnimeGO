using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserAnimeGO.Constants
{
    public class AnimesFilterConstants
    {
        public class Names
        {
            public const string Genres = "genres";
            public const string Type = "type";
            public const string Dubbing = "dubbing";
            public const string Status = "status";
            public const string RatingMPAA = "ratingMPAA";
        }

        public class LogicalOperators
        {
            public const string Or = "or";
            public const string And = "and";
        }
    }
}
