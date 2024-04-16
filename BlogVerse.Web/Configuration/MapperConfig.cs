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
        }
    }
}
