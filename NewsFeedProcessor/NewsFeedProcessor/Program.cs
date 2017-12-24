using System;
using AutoMapper;
using NewsFeedProcessor.Registrations;

namespace NewsFeedProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Please enter the syndication Url, type q to exit");
                var url = Console.ReadLine();
                if (url == "q") break;

                InitializeMapper();
                var result = new FeedProcessor().ProcessFeed(url);
                if (result == ProcessingStatus.NoChangeSinceLastUpdate) Console.WriteLine("No changes to the feed since last update");
            }
        }

        private static void InitializeMapper()
        {
            Mapper.Initialize(config => config.AddProfile<NewsFeedProfile>());
        }
    }
}
