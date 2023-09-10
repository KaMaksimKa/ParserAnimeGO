namespace ParserAnimeGO.Models
{
    public class AnimeCommentFromParser
    {
        public long IdForComments { get; set; }

        public long? CommentId { get; set; }

        public string? Comment { get; set; }

        public string? AuthorName { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public int? Score { get; set; }

        public long? ParentCommentId { get; set; }
    }
}
