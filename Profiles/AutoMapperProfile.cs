using AutoMapper;
using GoDecola.API.Entities;
using GoDecola.API.DTOs;
using GoDecola.API.DTOs.UserDTOs;
using GoDecola.API.DTOs.TravelPackageDTOs;
using GoDecola.API.DTOs.ReservationDTOs;

namespace GoDecola.API.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<CreateUserDTO, User>();

            CreateMap<TravelPackage, TravelPackageDTO>();
            CreateMap<CreateTravelPackageDTO, TravelPackage>();

            CreateMap<HotelAmenities, HotelAmenitiesDTO>();

            CreateMap<Reservation, ReservationDTO>();
            CreateMap<CreateReservationDTO, Reservation>();

            CreateMap<Guests, GuestsDTO>();
        }
    }
}
