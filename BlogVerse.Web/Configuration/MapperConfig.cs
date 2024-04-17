using AutoMapper;
using BlogVerse.Web.Models.Domain;
using BlogVerse.Web.Models.ViewModels;

namespace BlogVerse.Web.Configuration
{
    public class MapperConfig: Profile
    {
        public MapperConfig()
        {
            CreateMap<Tag, AddTagRequest>().ReverseMap();
            CreateMap<Tag, EditTagRequest>().ReverseMap();
            CreateMap<Tag, TagRequest>().ReverseMap();
            CreateMap<BlogPost, AddBlogPostRequest>().ReverseMap();
            CreateMap<BlogPost, BlogPostRequest>().ReverseMap();
            CreateMap<BlogPost, EditBlogPostRequest>().
                ForMember(dest => dest.Tags, opt => opt.Ignore());

            CreateMap<EditBlogPostRequest, BlogPost>();

        }
    }
}
