using System;
using System.ServiceModel.Syndication;

namespace NewsFeedProcessor.Models
{
    class NewsFeedItem
    {
        public int NewsFeedItemId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public int NewsFeedId { get; set; }
        public NewsFeed NewsFeed { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string BaseUri { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
    }
}
