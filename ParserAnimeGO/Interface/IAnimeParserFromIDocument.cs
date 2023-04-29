using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ParserAnimeGO.Models;

namespace ParserAnimeGO.Interface
{
    public interface IAnimeParserFromIDocument
    {
        public List<PartialAnimeData> GetPartialAnime(IDocument document);
        public MainAnimeData? GetMainDataAnime(IDocument document);
        public ShowAnimeData? GetShowDataAnime(IDocument document);
        public DubbingAnimeData? GetDubbingDataAnimeFromPlayerAsync(IDocument document);
        public List<AnimeNotificationFromParser> GetAnimeNotificationsFromParserAsync(IDocument document);

    }
}
