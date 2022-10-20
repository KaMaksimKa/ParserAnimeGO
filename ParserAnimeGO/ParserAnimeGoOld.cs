using System.Net;
using AngleSharp;
using AngleSharp.Dom;
using Newtonsoft.Json.Linq;
using ParserAnimeGO.AnimeData;

namespace ParserAnimeGO
{
    public class ParserAnimeGoOld: IDisposable
    {
        public string DefaultUrl { get; set; } = "https://animego.org/anime";
        private readonly IBrowsingContext _context;
        private HttpClient _httpClient;
        private int _timeBetweenRequest;
        public bool IsParsingNow { get; private set; }
        public string Cookies { get; set; } = "_ym_uid=1655761454412847163; _ym_d=1655761454; _ym_visorc=b; _ym_isad=2; _ga=GA1.2.647020422.1655761454; _gid=GA1.2.1695931329.1655761454; _gat_gtag_UA_111104961_1=1; __ddgid_=CYJlUGx7ZzZXXEev; __ddgmark_=TbWy6j7zaOdKURYU; __ddg5_=pi78mr5CeRXzVzkn; __ddg2_=bWehJXW1opppRaB9; __ddg1_=4AVL1fqjJqxZV0sKWMHW; device_view=full";
        public bool IsCookiesGood { get; private set; } = true;
        public int NeedToDo { get; private set; } = -1;
        public int Done { get; private set; } = -1;
        public string Description { get; private set; } = String.Empty;
        public string CurrentParsingUrl { get; private set; } = String.Empty;
        public string CurrentRequestsHeaders { get; private set; } = String.Empty;
        public string CurrentRequestsHeadersAfterResponse { get; private set; } = String.Empty;
        public HttpStatusCode? CurrentHttpStatusCode { get; private set; }
        public ParserAnimeGoOld()
        {
            _timeBetweenRequest = 1000;
            _context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            _httpClient = new HttpClient();
        }
        public async Task<List<AnimeFromParser>> GetFullAnimeFromDefaultUrlAsync()
        {
            return await GetFullAnimeFromUrlAsync(DefaultUrl);
        }
        public async Task<List<AnimeFromParser>> GetFullAnimeFromUrlAsync(string url)
        {
            if (IsParsingNow)
            {
                return new List<AnimeFromParser>();
            }
            var animeList =await GetPartialAnimeFromUrlAsync(url);
            NeedToDo = animeList.Count;
            Done = 0;
            Description = "Обновление частичной информации об ание до полной";
            int i = 0;
            foreach (var anime in animeList)
            {
                await UpdateAllDataAnime(anime);
                i += 1;
                Done = i;
            }
            return animeList;
        }
        public async Task<List<AnimeFromParser>> GetPartialAnimeFromDefaultUrlAsync()
        {
            return await GetPartialAnimeFromUrlAsync(DefaultUrl);
        }
        public async Task<List<AnimeFromParser>> GetPartialAnimeFromUrlAsync(string url)
        {
            if (IsParsingNow)
            {
                return new List<AnimeFromParser>();
            }
            else
            {
                IsParsingNow = true;
                Done = 0;
                Description = "Получение поверхностной информации об аниме";
            }
            

            if (!url.Contains("?"))
            {
                url += "?";
            }
            List<AnimeFromParser> animeList = new List<AnimeFromParser>();
            IDocument? page = null;

            int numberPage = 1;
            do
            {
                try
                {
                    string newUrl = url + $"&page={numberPage}";
                    CurrentParsingUrl = newUrl;
                    page = await GetDocumentFromHtmlAsync(newUrl);
                    foreach (var e in page.QuerySelectorAll(".animes-list-item"))
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
                        animeList.Add(new AnimeFromParser
                        {
                            Href = href,
                            IdFromAnimeGo = idFromAnimeGo,
                            TitleEn = nameEn,
                            TitleRu = nameRu,
                            Description = description,
                            Type = type,
                            Year = year,
                        });
                    }
                }
                finally
                {
                    page?.Close();
                }
                numberPage++;
                Done = animeList.Count;
            } while (page.StatusCode == HttpStatusCode.OK && numberPage<=55);

            IsParsingNow = false;
            CurrentParsingUrl = String.Empty;
            return animeList;
        }
        public async Task<AnimeFromParser> GetMainDataAnimeAsync(string? hrefAnime, int idFromAnimeGo)
        {
            var anime = new AnimeFromParser() { Href = hrefAnime, IdFromAnimeGo = idFromAnimeGo };
            await UpdateMainDataAnimeAsync(anime); 
            return anime;
        }
        public async Task<AnimeFromParser> GetShowDataAnimeAsync(int idFromAnimeGo)
        {
            var anime = new AnimeFromParser() {IdFromAnimeGo = idFromAnimeGo };
            await UpdateShowDataAnimeAsync(anime);
            return anime;
        }
        public async Task<AnimeFromParser> GetVoiceoverDataAnimeFromFirstEpisodeAsync(int idFromAnimeGo)
        {
            var anime = new AnimeFromParser() { IdFromAnimeGo = idFromAnimeGo };
            await UpdateVoiceoverDataAnimeFromFirstEpisodeAsync(anime);
            return anime;
        }
        public async Task<AnimeFromParser> GetAllDataAnime(string? hrefAnime, int idFromAnimeGo)
        {
            var anime = new AnimeFromParser() { Href = hrefAnime, IdFromAnimeGo = idFromAnimeGo };
            await UpdateAllDataAnime(anime);
            return anime;
        }
        public async Task UpdateMainDataAnimeAsync(AnimeFromParser anime)
        {
            if (anime.Href == null || IsParsingNow)
            {
                return;
            }
            else
            {
                IsParsingNow = true;
            }

            CurrentParsingUrl = anime.Href;
            using var page =await GetDocumentFromHtmlAsync(anime.Href);
            if (page.StatusCode == HttpStatusCode.OK)
            {
                double.TryParse(page.QuerySelector(".rating-value")?.Text().Trim(), out double rateResult);
                double? rate = rateResult == 0 ? null : rateResult;

                Dictionary<string, string?> dictionary = new Dictionary<string, string?>();
                if (page.QuerySelector(".anime-info")?.QuerySelectorAll("dt") is {} elements)
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
                List<string> studios = studioValue?.Split(",").Select(s => s.Trim() ).ToList() ??
                                       new List<string>();
                dictionary.TryGetValue("Длительность", out string? duration);
                dictionary.TryGetValue("Следующий эпизод", out string? nextEpisode);

                dictionary.TryGetValue("Озвучка ", out string? voiceoverValue);
                List<string> voiceovers = voiceoverValue?.Split(",").Select(v => v.Trim()).ToList() ??
                                          new List<string>();

                anime.Rate = rate;
                anime.CountEpisode = countEpisode;
                anime.Status = status;
                anime.Genres = genres;
                anime.MpaaRate = mpaaRate;
                anime.Studios = studios;
                anime.Duration = duration;
                anime.NextEpisode = nextEpisode;
                anime.Dubbing = anime.Dubbing.Union(voiceovers).ToList();

            }
            CurrentParsingUrl = String.Empty;
            IsParsingNow = false;
        }
        public async Task UpdateShowDataAnimeAsync(AnimeFromParser anime)
        {
            if (anime.IdFromAnimeGo == null || IsParsingNow)
            {
                return;
            }
            else
            {
                IsParsingNow = true;
            }

            Dictionary<string, string?> dictionary = new Dictionary<string, string?>();
            string url = $"https://animego.org/animelist/{anime.IdFromAnimeGo}/show";
            CurrentParsingUrl = url;
            using var doc = await GetDocumentFromJsonAsync(url);
            if (doc.StatusCode == HttpStatusCode.OK)
            {
                foreach (var e in doc.QuerySelectorAll("tr").Skip(1))
                {
                    dictionary.Add(e.QuerySelectorAll("td")[2].Text().Trim(), e.QuerySelectorAll("td")[0].Text().Trim());
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

            anime.Completed = completed;
            anime.Planned = planned;
            anime.Dropped = dropped;
            anime.OnHold = onHold;
            anime.Watching = watching;

            CurrentParsingUrl = String.Empty;
            IsParsingNow = false;
        }
        public async Task UpdateVoiceoverDataAnimeFromFirstEpisodeAsync(AnimeFromParser anime)
        {
            if (anime.IdFromAnimeGo == null || IsParsingNow)
            {
                return;
            }
            else
            {
                IsParsingNow = true;
            }

            List<string> list = new List<string>();
            string url = $"https://animego.org/anime/{anime.IdFromAnimeGo}/player?_allow=true";
            CurrentParsingUrl = url;
            using var doc = await GetDocumentFromJsonAsync(url);
            if (doc.StatusCode == HttpStatusCode.OK)
            {
                if (doc.QuerySelector("#video-dubbing") is { } selector)
                {
                    foreach (var e in selector.QuerySelectorAll(".video-player-toggle-item"))
                    {
                        list.Add(e.Text().Trim() );
                    }
                }
            }
            anime.Dubbing = anime.Dubbing.Union(list).ToList();

            CurrentParsingUrl = String.Empty;
            IsParsingNow = false;
        }
        public async Task UpdateAllDataAnime(AnimeFromParser anime)
        {
            await UpdateMainDataAnimeAsync(anime);
            await UpdateShowDataAnimeAsync(anime);
            await UpdateVoiceoverDataAnimeFromFirstEpisodeAsync(anime);
        }
        private async Task<IDocument> GetDocumentFromHtmlAsync(string url)
        {
            await Task.Delay(_timeBetweenRequest);
            HttpResponseMessage message = await GetResponseMessage(GetRequestMessage(url, Cookies, false));
            while (message.StatusCode == HttpStatusCode.TooManyRequests)
            {
                await Task.Delay(5000);
                message = await GetResponseMessage(GetRequestMessage(url, Cookies, false));
            }
            var html = await message.Content.ReadAsStringAsync();
            return await _context.OpenAsync(req =>
            {
                req.Content(html);
                req.Status(message.StatusCode);
            });
        }
        private async Task<IDocument> GetDocumentFromJsonAsync(string url)
        {
            await Task.Delay(_timeBetweenRequest);
            HttpResponseMessage message = await GetResponseMessage(GetRequestMessage(url, Cookies,true));
            while (message.StatusCode == HttpStatusCode.TooManyRequests)
            {
                await Task.Delay(5000);
                message = await GetResponseMessage(GetRequestMessage(url, Cookies, true));
            }

            var text = await message.Content.ReadAsStringAsync();
            JToken jToken = JToken.Parse(text);
            var html = jToken.Last?.Last?.ToString();

            return await _context.OpenAsync(req =>
            {
                req.Content(html);
                req.Status(message.StatusCode);
            });
        }
        public async Task<Stream?> GetStreamFromUrl(string? url)
        {
            if (url == null)
            {
                return null;
            }
            await Task.Delay(_timeBetweenRequest);
            var response = await GetResponseMessage(GetRequestMessage(url, Cookies, false));
            while (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                await Task.Delay(5000);
                response = await GetResponseMessage(GetRequestMessage(url, Cookies, false));
            }
            return await response.Content.ReadAsStreamAsync();
        }
        public async Task<Stream?> GetSteamPhotoFromAnimeHref(string? hrefAnime)
        {
            if (hrefAnime == null || IsParsingNow)
            {
                return null;
            }
            else
            {
                IsParsingNow = true;
            }

            CurrentParsingUrl = hrefAnime;
            using var page = await GetDocumentFromHtmlAsync(hrefAnime);
            if (page.StatusCode == HttpStatusCode.OK)
            {
                var url = page.QuerySelector(".anime-poster")?.QuerySelector("img")?.GetAttribute("src")?.Trim();
                CurrentParsingUrl = url??"";
                var stream= await GetStreamFromUrl(url);

                IsParsingNow = false;
                CurrentParsingUrl = String.Empty;
                return stream;
            }
            else
            {
                IsParsingNow = false;
                CurrentParsingUrl = String.Empty;
                return null;
            }
        }
        private HttpRequestMessage GetRequestMessage(string url, string cookies,bool isJson)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };
            request.Headers.Add("User-Agent", " Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.160 YaBrowser/22.5.2.615 Yowser/2.5 Safari/537.36");
            request.Headers.Add("Cookie", cookies);
            if (isJson)
            {
                request.Headers.Add("x-requested-with", "XMLHttpRequest");
            }

            CurrentRequestsHeaders = request.Headers.ToString();


            return request;
        }
        private async Task<HttpResponseMessage> GetResponseMessage(HttpRequestMessage requestMessage)
        {
            var response =await _httpClient.SendAsync(requestMessage);
            CurrentRequestsHeadersAfterResponse = response.RequestMessage.Headers.ToString();
            IsCookiesGood = response.StatusCode != HttpStatusCode.Forbidden;
            CurrentHttpStatusCode = response.StatusCode;
            return response;
        }
        public void Dispose()
        {
            _context.Dispose();
            _httpClient.Dispose();
        }
    }
}
