using ParserAnimeGO.Interface;
using HttpMethod = System.Net.Http.HttpMethod;

namespace ParserAnimeGO
{
    public class RequestParserFactory: IRequestParserFactory
    {
        public string? Cookies { get; set; }

        public string? UserAgent { get; set; } 
        
        public HttpRequestMessage GetHtmlRequestMessage(Uri uri)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            };
            request.Headers.UserAgent.ParseAdd(UserAgent);
            if (Cookies != null)
            {
                request.Headers.Add("Cookie", Cookies);
            }
            return request;
        }

        public HttpRequestMessage GetJsonRequestMessage(Uri uri)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            };
            request.Headers.UserAgent.ParseAdd(UserAgent);
            if (Cookies != null)
            {
                request.Headers.Add("Cookie", Cookies);
            }
            request.Headers.Add("x-requested-with", "XMLHttpRequest");
            return request;
        }
    }
}
