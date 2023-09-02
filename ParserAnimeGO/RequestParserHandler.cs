using System.Net;
using AngleSharp;
using AngleSharp.Dom;
using Newtonsoft.Json.Linq;
using ParserAnimeGO.Interface;

namespace ParserAnimeGO
{
    public class RequestParserHandler:IRequestParserHandler
    {
        public TimeSpan TimeBetweenRequest { get; set; } = TimeSpan.FromSeconds(1);
        private readonly HttpClient _httpClient;
        private readonly IBrowsingContext _browsingContext;

        public RequestParserHandler()
        {
            _browsingContext = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            _httpClient = new HttpClient(); 
        }

        public async Task<IDocument> SendHtmlRequestAsync(HttpRequestMessage request)
        {
            using var response = await GetResponseWithoutTooManyRequests(request);
            var html = await response.Content.ReadAsStringAsync();
            return await _browsingContext.OpenAsync(req =>
            {
                req.Content(html);
                req.Status(response.StatusCode);
            });
        }
        
        public async Task<IDocument> SendJsonRequestAsync(HttpRequestMessage request)
        {
            using var response = await GetResponseWithoutTooManyRequests(request);

            var jsonText = await response.Content.ReadAsStringAsync();
            JToken jToken = JToken.Parse(jsonText);
            var html = jToken.Last?.Last?.ToString();

            return await _browsingContext.OpenAsync(req =>
            {
                req.Content(html);
                req.Status(response.StatusCode);
            });
        }

        public async Task<Stream> SendImageRequestAsync(HttpRequestMessage request)
        {
            using var response = await GetResponseWithoutTooManyRequests(request);
            var stream = new MemoryStream();
            await (await response.Content.ReadAsStreamAsync()).CopyToAsync(stream);
            return stream;
        }

        private async Task<HttpResponseMessage> GetResponseWithoutTooManyRequests(HttpRequestMessage request)
        {
            await Task.Delay(TimeBetweenRequest);
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            while (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                await Task.Delay(TimeBetweenRequest * 5);
                response = await _httpClient.SendAsync(request);
            }
            return response;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _browsingContext.Dispose();
        }
    }
}
