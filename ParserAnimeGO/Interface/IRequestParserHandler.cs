using AngleSharp.Dom;

namespace ParserAnimeGO
{
    public interface IRequestParserHandler:IDisposable
    {
        public Task<IDocument> SendHtmlRequestAsync(HttpRequestMessage request);
        public Task<IDocument> SendJsonRequestAsync(HttpRequestMessage request);
    }
}
