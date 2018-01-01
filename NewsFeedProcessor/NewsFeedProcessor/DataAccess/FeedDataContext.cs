using System.Data.Entity;
using System.ServiceModel.Syndication;
using NewsFeedProcessor.Models;

namespace NewsFeedProcessor.DataAccess
{
    public class FeedDataContext : DbContext
    {
        public FeedDataContext() : base("NewsFeedRepository")
        {
        }

        public DbSet<NewsFeed> NewsFeeds { get; set; }
        public DbSet<NewsFeedItem> NewsFeedItems { get; set; }

    }
}
