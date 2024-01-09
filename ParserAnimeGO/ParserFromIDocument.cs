using AngleSharp.Dom;
using ParserAnimeGO.Interface;
using System.Net;
using AngleSharp;
using System.Globalization;
using ParserAnimeGO.Models.ParserModels;
using System.Numerics;

namespace ParserAnimeGO
{
    public class ParserFromIDocument : IAnimeParserFromIDocument
    {
        public List<PartialAnimeData> GetPartialAnime(IDocument document)
        {
            List<PartialAnimeData> animeList = new List<PartialAnimeData>();

            if (document.StatusCode != HttpStatusCode.OK)
            {
                return animeList;
            }

            foreach (var e in document.QuerySelectorAll(".animes-list-item"))
            {
                string? href = e.QuerySelector(".h5")?.QuerySelector("a")?.GetAttribute("href")?.Trim();
                long.TryParse(href?.Split("-")[^1], out var idFromAnimeGoResult);
                long? animeId = idFromAnimeGoResult == 0 ? null : idFromAnimeGoResult;

                var nameRu = e.QuerySelector(".h5 a")?.Text().Trim();

                var nameEn = e.QuerySelector(".text-gray-dark-6 ")?.Text().Trim();

                var type = e.QuerySelector("span")?.QuerySelector("a")?.Text().Trim();

                int.TryParse(e.QuerySelector(".anime-year")?.QuerySelector("a")?.Text().Trim(), out int yearResult);
                int? year = yearResult == 0 ? null : yearResult;

                var description = e.QuerySelector(".description")?.Text().Trim();

                if (animeId != null)
                {
                    animeList.Add(new PartialAnimeData
                    {
                        Href = href,
                        AnimeId = animeId.Value,
                        TitleEn = nameEn,
                        TitleRu = nameRu,
                        Description = description,
                        Type = type,
                        Year = year,
                    });
                }

            }


            return animeList;
        }

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

            long.TryParse(href?.Split("-")[^1], out var idFromAnimeGoResult);
            long? animeId = idFromAnimeGoResult == 0 ? null : idFromAnimeGoResult;

            double.TryParse(document.QuerySelector(".rating-value")?.Text().Replace(",", ".").Trim(), CultureInfo.InvariantCulture, out double rateResult);
            double? rate = rateResult == 0 ? null : rateResult;


            var imgId = document
                .QuerySelector(".anime-poster")
                ?.QuerySelector("img")
                ?.GetAttribute("src")
                ?.Trim()
                ?.Split("/").Last()
                .Split(".").First();


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
            nextEpisode = string.Join(" ", nextEpisode?.Split(new[] { " ", "\n" }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string?>());

            dictionary.TryGetValue("Озвучка", out string? voiceoverValue);
            List<string> voiceovers = voiceoverValue?.Split(",").Select(v => v.Trim()).ToList() ??
                                      new List<string>();

            long.TryParse(document.QuerySelector("#begin-comments")?.GetAttribute("data-thread-init"),
                out var idForCommentsResult);
            long? idForComments = idForCommentsResult == 0 ? null : idForCommentsResult;

            return new MainAnimeData
            {
                AnimeId = animeId,
                Rate = rate,
                CountEpisode = countEpisode,
                Status = status,
                Genres = genres,
                MpaaRate = mpaaRate,
                Studios = studios,
                Duration = duration,
                NextEpisode = nextEpisode,
                Dubbing = voiceovers,
                ImgIdFromAnimeGo = imgId,
                IdForComments = idForComments,

            };

        }

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

        public List<AnimeNotificationData> GetAnimeNotificationsData(IDocument document)
        {
            var animeNotifications = new List<AnimeNotificationData>();

            if (document.StatusCode != HttpStatusCode.OK)
            {
                return animeNotifications;
            }

            var lastUpdateItems = document.GetElementsByClassName("last-update").FirstOrDefault()
                    ?.GetElementsByClassName("last-update-item");
            if (lastUpdateItems == null)
            {
                return animeNotifications;
            }

            foreach (var lastUpdateItem in lastUpdateItems)
            {
                var titleRu = lastUpdateItem.GetElementsByClassName("last-update-title")
                    .FirstOrDefault()?.Text().Trim();

                var href = lastUpdateItem.GetAttribute("onclick")?.Replace("location.href=", "")
                    .Trim('\'');

                var textRight = lastUpdateItem.GetElementsByClassName("text-right").FirstOrDefault();
                var dubbing = textRight?.GetElementsByClassName("text-gray-dark-6")
                    .FirstOrDefault()?.Text().Trim('(', ')', ' ');

                int.TryParse(textRight?.GetElementsByClassName("font-weight-600").FirstOrDefault()?.Text().Split().FirstOrDefault(),
                    out var serialNumber);

                href = href != null ? "https://animego.org" + href : null;

                long.TryParse(href?.Split("-")[^1], out var idFromAnimeGoResult);
                long? animeId = idFromAnimeGoResult == 0 ? null : idFromAnimeGoResult;

                animeNotifications.Add(new AnimeNotificationData()
                {
                    AnimeId = animeId,
                    TitleRu = titleRu,
                    Dubbing = dubbing,
                    Href = href,
                    SerialNumber = serialNumber == 0 ? null : serialNumber
                });
            }

            return animeNotifications;
        }

        public List<AnimeCommentData> GetAnimeCommentsData(IDocument document)
        {
            var animeComments = new List<AnimeCommentData>();




            if (document.StatusCode != HttpStatusCode.OK)
            {
                return animeComments;
            }

            var comments = document.QuerySelectorAll(".comment")
                .Except(document.QuerySelectorAll(".children .comment"))
                .ToCollection();

            foreach (var comment in comments)
            {
                var animeComment = GetAnimeComment(comment);
                animeComments.Add(animeComment);

                var children = comment.QuerySelectorAll(".children .comment");

                foreach (var child in children)
                {
                    var childAnimeComment = GetAnimeComment(child);
                    childAnimeComment.ParentCommentId = animeComment.CommentId;
                    animeComments.Add(childAnimeComment);
                }
            }

            return animeComments;
        }

        public List<EpisodeData> GetEpisodesData(IDocument document)
        {
            var episodeDataList = new List<EpisodeData>();

            if (document.StatusCode == HttpStatusCode.OK)
            {
                foreach (var item in document.QuerySelectorAll(".video-player-bar-series-item"))
                {
                    episodeDataList.Add(new EpisodeData()
                    {
                        EpisodeNumber = int.TryParse(item.GetAttribute("data-episode"), out var parsedNumber) ? parsedNumber : null,
                        EpisodeId = long.TryParse(item.GetAttribute("data-id"), out var parsedSeriesId) ? parsedSeriesId : null,
                        EpisodeReleased = item.GetAttribute("data-episode-released"),
                        EpisodeTitle = item.GetAttribute("data-episode-title"),
                        EpisodeDescription = item.GetAttribute("data-episode-description"),
                        EpisodeType = int.TryParse(item.GetAttribute("data-episode-type"), out var parsedType) ? parsedType : null,
                    });
                }
            }

            return episodeDataList;
        }

        public List<EpisodeWatchData> GetEpisodeWatchData(IDocument document)
        {
            var episodeWatchDataList = new List<EpisodeWatchData>();

            if (document.StatusCode == HttpStatusCode.OK)
            {
                var videoDubbingDiv = document.QuerySelector("#video-dubbing");
                var videoPlayersDiv = document.QuerySelector("#video-players");

                if (videoDubbingDiv != null && videoPlayersDiv != null)
                {
                    var dubbing = new Dictionary<int, string?>();

                    foreach (var item in videoDubbingDiv.QuerySelectorAll(".video-player-toggle-item"))
                    {
                        if (int.TryParse(item.GetAttribute("data-dubbing"), out int dubbingId))
                        {
                            var dubbingName = item.QuerySelector(".video-player-toggle-item-name")?.Text().Trim();
                            dubbing.Add(dubbingId, dubbingName);
                        }
                    }

                    foreach (var item in videoPlayersDiv.QuerySelectorAll(".video-player-toggle-item"))
                    {
                        if (int.TryParse(item.GetAttribute("data-provide-dubbing"), out int dubbingId))
                        {
                            episodeWatchDataList.Add(new EpisodeWatchData()
                            {
                                ProviderName = item.QuerySelector(".video-player-toggle-item-name")?.Text().Trim(),
                                DubbingName = dubbing[dubbingId],
                                VideoPlayerLink = item.GetAttribute("data-player")
                            });
                        }
                    }
                }
            }

            return episodeWatchDataList;
        }

        private AnimeCommentData GetAnimeComment(IElement comment)
        {
            long.TryParse(comment.GetAttribute("data-id"), out var commentId);
            var authorName = comment.QuerySelector(".comment-author .text-truncate a")?.GetAttribute("title");
            var commentText = comment.QuerySelector(".comment-text div")?.TextContent.Trim();
            int.TryParse(comment.QuerySelector(".comment-actions .d-inline-flex .mr-3")?.TextContent, out var score);
            DateTimeOffset.TryParse(comment.QuerySelector(".comment-author .time time")?.GetAttribute("datetime"),
                out var createdDate);

            return new AnimeCommentData()
            {
                CommentId = commentId,
                Comment = commentText,
                CreatedDate = createdDate,
                ParentCommentId = null,
                Score = score,
                AuthorName = authorName
            };
        }
    }
}
