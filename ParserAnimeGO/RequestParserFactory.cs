using AngleSharp.Io;
using ParserAnimeGO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HttpMethod = System.Net.Http.HttpMethod;

namespace ParserAnimeGO
{
    public class RequestParserFactory: IRequestParserFactory
    {
        public string? Cookies { get; set; }

        public string UserAgent { get; set; } =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.160 YaBrowser/22.5.2.615 Yowser/2.5 Safari/537.36";

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
