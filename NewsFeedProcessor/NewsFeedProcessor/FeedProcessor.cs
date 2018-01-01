using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using AutoMapper;
using log4net;
using NewsFeedProcessor.DataAccess;
using NewsFeedProcessor.Models;

namespace NewsFeedProcessor
{
    public enum ProcessingStatus
    {
        [Description("Feed processed successfully")]
        Processed,
        [Description("No changes to feed since last update")]
        NoChangeSinceLastUpdate,
        [Description("An error was encoutered processing the feed. Refer the log for more information")]
        ErrorProcessingFeed
    }
    class FeedProcessor
    {
        private readonly ILog _logger;

        internal FeedProcessor()
        {
            _logger = LogManager.GetLogger("NewsFeedLogger");
        }

        public ProcessingStatus ProcessFeed(string url)
        {
            ProcessingStatus status = ProcessingStatus.NoChangeSinceLastUpdate;
            try
            {
                using (var context = new FeedDataContext())
                {
                    var webRequest = (HttpWebRequest)WebRequest.Create(url);
                    webRequest.Accept = "application/xml";
                    using (var response = webRequest.GetResponse())
                    using (var stream = response.GetResponseStream())
                        if (stream != null)
                            using (var reader = new MyXmlReader(stream))
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
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                status = ProcessingStatus.ErrorProcessingFeed;
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
                var existingItem = context.NewsFeedItems.FirstOrDefault(f => f.Identifier == item.Identifier);

                if (item.LastUpdatedTime >= existingItem?.LastUpdatedTime) continue;

                items.Add(existingItem != null ? Mapper.Map(item, existingItem) : item);
            }

            return items;
        }
    }
}
