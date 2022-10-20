using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserAnimeGO.Interface
{
    public interface IAnimeGoUriFactory
    {
        public Uri GetAnimesByPageUri(int numberOfPage);

        public Uri GetShowDataAnimeByIdFromAnimeGoUri(int idFromAnimeGo);

        public Uri GetVoiceoverDataAnimeByIdFromAnimeGoUri(int idFromAnimeGo);
    }
}
