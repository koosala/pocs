using System.Linq;
using System.ServiceModel.Syndication;
using AutoMapper;
using NewsFeedProcessor.Models;

namespace NewsFeedProcessor.Registrations
{
    public class NewsFeedProfile : Profile
    {
        public NewsFeedProfile()
        {
            CreateMap<SyndicationFeed, NewsFeed>()
                .ForMember(n => n.BaseUri, s => s.MapFrom(s2 => s2.BaseUri.AbsoluteUri))
                .ForMember(n => n.Title, s => s.MapFrom(s2 => s2.Title.Text))
                .ForMember(n => n.Description, s => s.MapFrom(s2 => s2.Description.Text));
            CreateMap<SyndicationItem, NewsFeedItem>()
                .ForMember(n => n.Author, s => s.MapFrom(s2 => s2.Authors.FirstOrDefault().Name))
                .ForMember(n => n.BaseUri, s => s.MapFrom(s2 => s2.BaseUri.AbsoluteUri))
                .ForMember(n => n.Title, s => s.MapFrom(s2 => s2.Title.Text))
                .ForMember(n => n.Summary, s => s.MapFrom(s2 => s2.Summary.Text))
                .ForMember(n => n.Identifier, s => s.MapFrom(s2 => s2.Id));
            CreateMap<NewsFeed, NewsFeed>()
                .ForMember(n => n.NewsFeedId, s => s.Ignore());
        }
    }
}
