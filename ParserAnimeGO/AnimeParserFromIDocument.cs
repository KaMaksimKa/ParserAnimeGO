using AngleSharp.Dom;
using ParserAnimeGO.AnimeData;
using ParserAnimeGO.Interface;
using System.Net;

namespace ParserAnimeGO
{
    public class AnimeParserFromIDocument: IAnimeParserFromIDocument
    {
        /*Получение частичной информации об аниме со страницы с несколькоми аниме с сайта AnimeGO:
          Href,IdFromAnimeGo,TitleEn,TitleRu,Description,Type,Year*/
        public List<PartialAnimeData> GetPartialAnime(IDocument document)
        {
            List<PartialAnimeData> animeList = new List<PartialAnimeData>();

            if (document.StatusCode == HttpStatusCode.OK)
            {
                foreach (var e in document.QuerySelectorAll(".animes-list-item"))
                {
                    string? href = e.QuerySelector(".h5")?.QuerySelector("a")?.GetAttribute("href")?.Trim();
                    int.TryParse(href?.Split("-")[^1], out int idFromAnimeGoResult);
                    int? idFromAnimeGo = idFromAnimeGoResult == 0 ? null : idFromAnimeGoResult;

                    var nameRu = e.QuerySelector(".h5")?.Text().Trim();

                    var nameEn = e.QuerySelector(".text-gray-dark-6 ")?.Text().Trim();

                    var type = e.QuerySelector("span")?.QuerySelector("a")?.Text().Trim();

                    int.TryParse(e.QuerySelector(".anime-year")?.QuerySelector("a")?.Text().Trim(), out int yearResult);
                    int? year = yearResult == 0 ? null : yearResult;

                    var description = e.QuerySelector(".description")?.Text().Trim();

                    if (idFromAnimeGo != null)
                    {
                        animeList.Add(new PartialAnimeData
                        {
                            Href = href,
                            IdFromAnimeGo = idFromAnimeGo.Value,
                            TitleEn = nameEn,
                            TitleRu = nameRu,
                            Description = description,
                            Type = type,
                            Year = year,
                        });
                    }

                }
            }
            
            return animeList;
        }


        /*Получение основной информации об аниме со страницы аниме с сайта AnimeGO:
         Rate, CountEpisode, Status, Genres, MpaaRate, Studios,Duration,NextEpisode,Dubbing,ImgHref*/
        public MainAnimeData? GetMainDataAnime(IDocument document)
        {
            if (document.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            Dictionary<string, string?> dictionary = new Dictionary<string, string?>();

            string? href = (document.Head?
                .QuerySelectorAll("link"))?
                .FirstOrDefault(e => e.HasAttribute("rel") && e.GetAttribute("rel") == "canonical")?
                .GetAttribute("href")?.Trim();
            
            int.TryParse(href?.Split("-")[^1], out int idFromAnimeGoResult);

            double.TryParse(document.QuerySelector(".rating-value")?.Text().Trim(), out double rateResult);
            double? rate = rateResult == 0 ? null : rateResult;

            var imgUrl = document.QuerySelector(".anime-poster")?.QuerySelector("img")?.GetAttribute("src")?.Trim();


            if (document.QuerySelector(".anime-info")?.QuerySelectorAll("dt") is { } elements)
            {
                foreach (var e in elements)
                {
                    dictionary.Add(e.Text().Trim(), e.NextElementSibling?.Text().Trim());
                }
            }
            

            dictionary.TryGetValue("Эпизоды", out string? countEpisodeValue);
            int.TryParse(countEpisodeValue?.Split("/")[^1], out int countEpisodeResult);
            int? countEpisode = countEpisodeResult == 0 ? null : countEpisodeResult;

            dictionary.TryGetValue("Статус", out string? status);

            dictionary.TryGetValue("Жанр", out string? genresValue);
            List<string> genres = genresValue?.Split(",").Select(g => g.Trim()).ToList() ??
                                  new List<string>();

            dictionary.TryGetValue("Рейтинг MPAA", out string? mpaaRate);
            dictionary.TryGetValue("Студия", out string? studioValue);
            List<string> studios = studioValue?.Split(",").Select(s => s.Trim()).ToList() ??
                                   new List<string>();
            dictionary.TryGetValue("Длительность", out string? duration);
            dictionary.TryGetValue("Следующий эпизод", out string? nextEpisode);

            dictionary.TryGetValue("Озвучка ", out string? voiceoverValue);
            List<string> voiceovers = voiceoverValue?.Split(",").Select(v => v.Trim()).ToList() ??
                                      new List<string>();



            return new MainAnimeData
            {
                IdFromAnimeGo = idFromAnimeGoResult,
                Rate = rate,
                CountEpisode = countEpisode,
                Status = status,
                Genres = genres,
                MpaaRate = mpaaRate,
                Studios = studios,
                Duration = duration,
                NextEpisode = nextEpisode,
                Dubbing = voiceovers,
                ImgHref = imgUrl
            };
            
        }
        

        /*Получение  информации об просмотрах у одного аниме:
          Completed, Planned, Dropped, OnHold, Watching*/
        public ShowAnimeData? GetShowDataAnime(IDocument document)
        {
            if (document.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            Dictionary<string, string?> dictionary = new Dictionary<string, string?>();
            
            if (document.StatusCode == HttpStatusCode.OK)
            {
                foreach (var element in document.QuerySelectorAll("tr").Skip(1))
                {
                    dictionary.Add(element.QuerySelectorAll("td")[2].Text().Trim(), element.QuerySelectorAll("td")[0].Text().Trim());
                }
            }

            dictionary.TryGetValue("Смотрю", out string? watchingValue);
            int.TryParse(watchingValue, out int watchingResult);
            int? watching = watchingResult == 0 ? null : watchingResult;

            dictionary.TryGetValue("Просмотрено", out string? completedValue);
            int.TryParse(completedValue, out int completedResult);
            int? completed = completedResult == 0 ? null : completedResult;

            dictionary.TryGetValue("Брошено", out string? droppedValue);
            int.TryParse(droppedValue, out int droppedResult);
            int? dropped = droppedResult == 0 ? null : droppedResult;

            dictionary.TryGetValue("Отложено", out string? onHoldValue);
            int.TryParse(onHoldValue, out int onHoldResult);
            int? onHold = onHoldResult == 0 ? null : onHoldResult;

            dictionary.TryGetValue("Запланировано", out string? plannedValue);
            int.TryParse(plannedValue, out int plannedResult);
            int? planned = plannedResult == 0 ? null : plannedResult;

            
            return new ShowAnimeData
            {
                Completed = completed,
                Planned = planned,
                Dropped = dropped,
                OnHold = onHold,
                Watching = watching
        };

        }


        /*Получение  информации об озвучках у одного аниме у одной серии:
          Dubbing*/
        public DubbingAnimeData? GetDubbingDataAnimeFromPlayerAsync(IDocument document)
        {
            if (document.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            List<string> listDubbing = new List<string>();
            
            if (document.StatusCode == HttpStatusCode.OK)
            {
                if (document.QuerySelector("#video-dubbing") is { } selector)
                {
                    foreach (var e in selector.QuerySelectorAll(".video-player-toggle-item"))
                    {
                        listDubbing.Add(e.Text().Trim());
                    }
                }
            }

            return new DubbingAnimeData
            {
                Dubbing = listDubbing
            };
        }
    }
}
