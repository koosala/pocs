using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace NewsFeedProcessor.Models
{
    class NewsFeed
    {
        public string Title { get; set; }
        public int NewsFeedId { get; set; }
        public string BaseUri { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public string Description { get; set; }

        public IEnumerable<NewsFeedItem> Items { get; set; }
    }
}
