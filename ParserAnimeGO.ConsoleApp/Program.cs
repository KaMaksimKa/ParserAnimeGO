using AngleSharp;
using ParserAnimeGO;
using ParserAnimeGO.Interface;


IAnimeGoUriFactory uriFactory = new AnimeGoUriFactory();
IRequestParserFactory requestParserFactory = new RequestParserFactory();
IRequestParserHandler requestParserHandler = new RequestParserHandler();
IAnimeParserFromIDocument animeParserFromIDocument = new AnimeParserFromIDocument();
ParserAnimeGo parserAnimeGo = new ParserAnimeGo(requestParserHandler,requestParserFactory,animeParserFromIDocument,uriFactory);
var anime =await parserAnimeGo.GetFullAnimesByPageAsync(1);
Console.WriteLine(anime.Count);