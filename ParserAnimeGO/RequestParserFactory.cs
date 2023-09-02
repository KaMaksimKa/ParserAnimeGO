using ParserAnimeGO.Interface;
using HttpMethod = System.Net.Http.HttpMethod;

namespace ParserAnimeGO
{
    public class RequestParserFactory: IRequestParserFactory
    {
        public string? Cookies { get; set; }

        public string? UserAgent { get; set; }

        public HttpRequestMessage GetHtmlRequestMessage(Uri uri) => GetRequestWithDefaultHeaders(uri);

        public HttpRequestMessage GetJsonRequestMessage(Uri uri)
        {
            var request = GetRequestWithDefaultHeaders(uri);
            request.Headers.Add("x-requested-with", "XMLHttpRequest");
            return request;
        }

        public HttpRequestMessage GetImageRequestMessage(Uri uri) => GetRequestWithDefaultHeaders(uri);

        private HttpRequestMessage GetRequestWithDefaultHeaders(Uri uri)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            };

            if (!string.IsNullOrEmpty(UserAgent))
            {
                request.Headers.UserAgent.ParseAdd(UserAgent);
            }

            if (!string.IsNullOrEmpty(Cookies))
            {
                request.Headers.Add("Cookie", Cookies);
            }

            return request;
        }
    }
}
