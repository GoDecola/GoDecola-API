using AutoMapper;
using GoDecola.API.DTO;
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