

using ParserAnimeGO;

var requestHandler = new RequestParserHandler();
var requestFactory = new RequestParserFactory()
{
    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.160 YaBrowser/22.5.2.615 Yowser/2.5 Safari/537.36"
};
var parserDocument = new ParserFromIDocument();
var uriFactory = new AnimeGoUriFactory();
var animeParser = new ParserAnimeGo(requestHandler, requestFactory, parserDocument, uriFactory);

var animeComments = await animeParser.GetAnimeCommentsAsync(4034);
Console.OutputEncoding = System.Text.Encoding.UTF8;

foreach (var animeComment in animeComments)
{
    Console.WriteLine(animeComment.CommentId);
    Console.WriteLine(animeComment.AuthorName);
    Console.WriteLine(animeComment.CreatedDate);
    Console.WriteLine(animeComment.Score);
    Console.WriteLine(animeComment.Comment);
    Console.WriteLine(animeComment.ParentCommentId);
    Console.WriteLine("----------------------");
    Console.WriteLine();
}