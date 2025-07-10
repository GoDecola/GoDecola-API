using AutoMapper;
using GoDecola.API.DTO.Request;
using GoDecola.API.Model;

namespace GoDecola.API.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterRequestDTO, User>();
        }
    }
}