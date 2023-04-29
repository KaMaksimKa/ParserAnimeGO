using System.Text;
using ParserAnimeGO.Constants;
using ParserAnimeGO.Interface;
using ParserAnimeGO.Models;

namespace ParserAnimeGO
{
    public class AnimeGoUriFactory: IAnimeGoUriFactory
    {
        private static readonly Uri _baseUri = new Uri("https://animego.org");

        public Uri GetAnimes(AnimesArgs animeArgs)
        {
            var url = new StringBuilder("anime");


            if (animeArgs.Filters.Any() || animeArgs.YearFrom != null || animeArgs.YearTo != null)
            {
                url.Append($"/filter");

                if (animeArgs.YearFrom != null || animeArgs.YearTo != null)
                {
                    url.Append($"/year");

                    if (animeArgs.YearFrom != null)
                    {
                        url.Append($"-from-{animeArgs.YearFrom}");
                    }

                    if (animeArgs.YearTo != null)
                    {
                        url.Append($"-to-{animeArgs.YearTo}");
                    }
                }

                foreach (var filter in animeArgs.Filters)
                {
                    url.Append($"/{filter.Name}-is");

                    var isFirst = true;

                    foreach (var value in filter.Values)
                    {
                        if (isFirst)
                        {
                            url.Append($"-{value}");
                            isFirst = false;
                        }
                        else
                        {
                            url.Append($"-{filter.LogicalOperator}-{value}");
                        }
                    }
                    type -is -tv - or - movie
                }

            }



            if (animeArgs.PageNumber != null || animeArgs.Sort != null || animeArgs.Direction != null)
            {
                url.Append("?");

                if (animeArgs.PageNumber != null)
                {
                    url.Append($"page={animeArgs.PageNumber.Value}&");
                }

                if (animeArgs.Sort != null)
                {
                    url.Append($"sort={animeArgs.Sort}&");
                }

                if (animeArgs.Direction != null)
                {
                    url.Append($"direction={animeArgs.Direction}&");
                }
            }
            return new Uri(_baseUri, url.ToString());
        }

        public Uri GetShowDataAnimeById(int idFromAnimeGo)
        {
            string url = $"https://animego.org/animelist/{idFromAnimeGo}/show";
            return new Uri(url);
        }

        public Uri GetVoiceoverDataAnimeById(int idFromAnimeGo)
        {
            string url = $"https://animego.org/anime/{idFromAnimeGo}/player?_allow=true";
            return new Uri(url);
        }

        public Uri GetAnimeNotifications()
        {
            return new Uri("https://animego.org");
        }

    }
}
