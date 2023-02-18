using AngleSharp;
using ParserAnimeGO;
using ParserAnimeGO.ConsoleApp.Data;
using ParserAnimeGO.ConsoleApp.Data.AnimeModels;
using ParserAnimeGO.Interface;


IAnimeGoUriFactory uriFactory = new AnimeGoUriFactory();
IRequestParserFactory requestParserFactory = new RequestParserFactory();
IRequestParserHandler requestParserHandler = new RequestParserHandler();
IAnimeParserFromIDocument animeParserFromIDocument = new AnimeParserFromIDocument();
ParserAnimeGo parserAnimeGo = new ParserAnimeGo(requestParserHandler,requestParserFactory,animeParserFromIDocument,uriFactory);
var animeFromParsers =await parserAnimeGo.GetFullAnimesByPageAsync(1);




ApplicationContext context = new ApplicationContext();
var animes = animeFromParsers.Select(Anime.FromAnimeFromParser).ToList();
foreach (var anime in animes)
{
    context.Genres.AttachRange(anime.Genres);
}
await context.Animes.AddRangeAsync(animes);
context.SaveChanges();
var genres = context.Genres.ToList();
Console.WriteLine(genres.Count);
foreach (var genre in genres.OrderBy(g=>g.Title))
{
    Console.WriteLine(genre.Title);
}