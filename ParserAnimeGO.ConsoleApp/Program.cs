

using AngleSharp.Browser;
using ParserAnimeGO;
using ParserAnimeGO.Models;

var requestHandler = new RequestParserHandler();
var requestFactory = new RequestParserFactory()
{
    Cookies = "",
    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.160 YaBrowser/22.5.2.615 Yowser/2.5 Safari/537.36"
};
var parserDocument = new ParserFromIDocument();
var uriFactory = new AnimeGoUriFactory();
var animeParser = new ParserAnimeGo(requestHandler, requestFactory, parserDocument, uriFactory);

var a = await animeParser.GetAllCommentsFromAnime(4048,2000);

Console.WriteLine(a.Count);