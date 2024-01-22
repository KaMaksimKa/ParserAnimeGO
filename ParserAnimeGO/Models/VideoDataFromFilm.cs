using ParserAnimeGO.Models.ParserModels;

namespace ParserAnimeGO.Models
{
    public class VideoDataFromFilm : VideoData
    {
        public long AnimeId { get; set; }

        public VideoDataFromFilm(VideoData videoData)
        {
            DubbingName = videoData.DubbingName;
            ProviderName = videoData.ProviderName;
            VideoPlayerLink = videoData.VideoPlayerLink;
        }
    }
}
