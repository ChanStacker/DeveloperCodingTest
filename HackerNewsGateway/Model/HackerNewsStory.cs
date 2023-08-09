using System;

namespace HackerNewsGateway.Model
{
    public class HackerNewsStory
    {
        public string Title { get; set; }
        public string Uri { get; set; }
        public string PostedBy { get; set; }
        public DateTime Time { get; set; }
        public int Score { get; set; }
        public int CommentCount { get; set; }
    }

    public class HackerNewsItem
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public string? Type { get; set; }
        public string? By { get; set; }
        public long Time { get; set; }
        public string? Text { get; set; }
        public bool Dead { get; set; }
        public int Parent { get; set; }
        public string? Poll { get; set; }
        public int[] Kids { get; set; }
        public string? Url { get; set; }
        public int Score { get; set; }
        public string? Title { get; set; }
        public string? Parts { get; set; }
        public int Descendants { get; set; }
    }
}
