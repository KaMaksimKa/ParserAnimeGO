using AngleSharp.Dom;

namespace ParserAnimeGO.Interface
{
    public interface IRequestParserHandler:IDisposable
    {
        public Task<IDocument> SendHtmlRequestAsync(HttpRequestMessage request);
        public Task<IDocument> SendJsonRequestAsync(HttpRequestMessage request);
        public Task<Stream> SendImageRequestAsync(HttpRequestMessage request);
    }
}
