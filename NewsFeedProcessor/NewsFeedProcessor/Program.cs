using System;
using AutoMapper;
using EnumsNET;
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
                Console.WriteLine(result.AsString(EnumFormat.Description));
            }
        }

        private static void InitializeMapper()
        {
            Mapper.Initialize(config => config.AddProfile<NewsFeedProfile>());
        }
    }
}
