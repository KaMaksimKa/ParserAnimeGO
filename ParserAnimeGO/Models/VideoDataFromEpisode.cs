using ParserAnimeGO.Models.ParserModels;

namespace ParserAnimeGO.Models
{
    public class VideoDataFromEpisode : VideoData
    {
        public long EpisodeId { get; set; }

        public VideoDataFromEpisode(VideoData videoData)
        {
            DubbingName = videoData.DubbingName;
            ProviderName = videoData.ProviderName;
            VideoPlayerLink = videoData.VideoPlayerLink;
        }
    }
}
