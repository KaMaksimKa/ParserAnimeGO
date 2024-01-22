using ParserAnimeGO.Interface;
using ParserAnimeGO.Models;
using System.Text;

namespace ParserAnimeGO
{
    public class AnimeGoUriFactory : IAnimeGoUriFactory
    {
        private static readonly Uri BaseUri = new Uri("https://animego.org");

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
            return new Uri(BaseUri, url.ToString());
        }

        public Uri GetShowDataAnime(long animeId)
        {
            return new Uri(BaseUri, $"animelist/{animeId}/show");
        }

        public Uri GetEpisodeData(long animeId)
        {
            return new Uri(BaseUri, $"anime/{animeId}/player?_allow=true");
        }

        public Uri GetAnimeNotifications()
        {
            return BaseUri;
        }

        public Uri GetAnimeImage(string imgIdFromAnimeGo)
        {
            return new Uri(BaseUri, $"upload/anime/images/{imgIdFromAnimeGo}.jpg");
        }

        public Uri GetAnimeComments(long idForComments, int numberOfPage, int limit)
        {
            return new Uri(BaseUri, $"comment/{idForComments}/1/show?view=list&page={numberOfPage}&limit={limit}");
        }

        public Uri GetVideoDatasFromEpisode(long episodeId)
        {
            return new Uri(BaseUri, $"anime/series?id={episodeId}");
        }

        public Uri GetVideoDataFromFilm(long animeId)
        {
            return new Uri(BaseUri, $"anime/{animeId}/player?_allow=true");
        }
    }
}
