using API.Contracts;
using API.Models;
using AutoMapper;

namespace API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, PostRequest>();
        }
    }
}
