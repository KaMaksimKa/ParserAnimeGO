using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserAnimeGO.Interface
{
    public interface IRequestParserFactory
    {
        HttpRequestMessage GetHtmlRequestMessage(Uri uri);
        HttpRequestMessage GetJsonRequestMessage(Uri uri);
        HttpRequestMessage GetImageRequestMessage(Uri uri);
    }
}
