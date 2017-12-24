using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using AutoMapper;
using NewsFeedProcessor.DataAccess;
using NewsFeedProcessor.Models;

namespace NewsFeedProcessor
{
    public enum ProcessingStatus
    {
        Processed,
        NoChangeSinceLastUpdate,
        IncorrectFeed
    }
    class FeedProcessor
    {
        public ProcessingStatus ProcessFeed(string url)
        {
            ProcessingStatus status = ProcessingStatus.NoChangeSinceLastUpdate;

            using (var context = new FeedDataContext())
            {
                using (var reader = XmlReader.Create(url))
                {
                    var feed = SyndicationFeed.Load(reader);
                    var newsFeed = GetTranslatedFeed(feed, context, url);

                    if (newsFeed != null)
                    {
                        context.NewsFeeds.AddOrUpdate(newsFeed);
                        context.NewsFeedItems.AddRange(newsFeed.Items);
                        context.SaveChanges();
                        status = ProcessingStatus.Processed;
                    }
                }
            }

            return status;
        }

        private NewsFeed GetTranslatedFeed(SyndicationFeed feed, FeedDataContext context, string url)
        {
            var newsFeed = Mapper.Map<NewsFeed>(feed);
            newsFeed.BaseUri = newsFeed.BaseUri ?? url;
            var existingFeed = context.NewsFeeds.FirstOrDefault(f => f.BaseUri == newsFeed.BaseUri);

            if (existingFeed?.LastUpdatedTime >= newsFeed.LastUpdatedTime) return null;

            existingFeed = existingFeed != null ? Mapper.Map(newsFeed, existingFeed) : newsFeed;
            existingFeed.Items = GetItems(feed.Items, context);
            return existingFeed;
        }

        private IEnumerable<NewsFeedItem> GetItems(IEnumerable<SyndicationItem> syndicationItems, FeedDataContext context)
        {
            List<NewsFeedItem> items = new List<NewsFeedItem>();
            var candidateItems = Mapper.Map<IEnumerable<NewsFeedItem>>(syndicationItems).Where(i => !string.IsNullOrEmpty(i.Summary));

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var item in candidateItems)
            {
                var existingItem = context.NewsFeedItems.FirstOrDefault(f => f.BaseUri == item.BaseUri);

                if (item.LastUpdatedTime >= existingItem?.LastUpdatedTime) continue;

                items.Add(existingItem != null ? Mapper.Map(item, existingItem) : item);
            }

            return items;
        }
    }
}
