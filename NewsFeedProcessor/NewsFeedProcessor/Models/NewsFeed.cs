using System;
using System.Collections.Generic;

namespace NewsFeedProcessor.Models
{
    public class NewsFeed
    {
        public string Title { get; set; }
        public int NewsFeedId { get; set; }
        public string BaseUri { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public string Description { get; set; }

        public IEnumerable<NewsFeedItem> Items { get; set; }
    }
}
