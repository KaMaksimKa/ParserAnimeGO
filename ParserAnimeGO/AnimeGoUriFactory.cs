using ParserAnimeGO.Interface;
using ParserAnimeGO.Models;
using System.Text;

namespace ParserAnimeGO
{
    public class AnimeGoUriFactory : IAnimeGoUriFactory
    {
        private static readonly Uri BaseUri = new Uri("https://animego.org");

        /// <summary>
        /// Получить url до страницы со всеми аниме
        /// </summary>
        /// <param name="animeArgs"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Получить url до информации о списках у пользователей для аниме
        /// </summary>
        /// <param name="idFromAnimeGo"></param>
        /// <returns></returns>
        public Uri GetShowDataAnimeById(long idFromAnimeGo)
        {
            return new Uri(BaseUri, $"animelist/{idFromAnimeGo}/show");
        }

        /// <summary>
        /// Получить url до информации о озвучке у первой серии аниме
        /// </summary>
        /// <param name="idFromAnimeGo"></param>
        /// <returns></returns>
        public Uri GetVoiceoverDataAnimeById(long idFromAnimeGo)
        {
            return new Uri(BaseUri, $"anime/{idFromAnimeGo}/player?_allow=true");
        }

        /// <summary>
        /// Получить url до страницы с информацией о выходе новых серий
        /// </summary>
        /// <returns></returns>
        public Uri GetAnimeNotifications()
        {
            return BaseUri;
        }

        /// <summary>
        /// Получить комментарии по отдельному аниме
        /// </summary>
        /// <param name="idForComments"></param>
        /// <param name="numberOfPage"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Uri GetAnimeComments(long idForComments, int numberOfPage, int limit)
        {
            return new Uri(BaseUri, $"comment/{idForComments}/1/show?view=list&page={numberOfPage}&limit={limit}");
        }

    }
}
