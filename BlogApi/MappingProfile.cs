using BlogApi.Contracts;
using AutoMapper;
using BlogApi.Contracts.Models.Posts;
using BlogApi.Data.Models;
using BlogApi.Contracts.Models.Users;
using static BlogApi.Contracts.Models.Tags.GetTagsModel;
using BlogApi.Contracts.Models.Tags;

namespace BlogApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, PostView>();
            CreateMap<AddPostModel, Post>();
            CreateMap<ApplicationUser, UserView>();
            CreateMap<ApplicationUser, AddUserModel>()
                .ForMember(x => x.Email,
                y => y.MapFrom(src => src.UserName));
            CreateMap<ApplicationUser, EditUserModel>();
            CreateMap<Tag, TagView>();
            CreateMap<Tag, AddTagModel>();
        }
    }
}
