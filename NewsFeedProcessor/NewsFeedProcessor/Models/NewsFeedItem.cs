using System;

namespace NewsFeedProcessor.Models
{
    public class NewsFeedItem
    {
        public int NewsFeedItemId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public int NewsFeedId { get; set; }
        public NewsFeed NewsFeed { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string BaseUri { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public string Identifier { get; set; }
    }
}
