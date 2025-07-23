using AutoMapper;
using GoDecola.API.DTOs;
using GoDecola.API.DTOs.UserDTOs;
using GoDecola.API.DTOs.TravelPackageDTOs;
using GoDecola.API.DTOs.ReservationDTOs;
using GoDecola.API.Entities;

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
            CreateMap<UpdateTravelPackageDTO, TravelPackage>();

            CreateMap<HotelAmenities, HotelAmenitiesDTO>();
            CreateMap<HotelAmenitiesDTO, HotelAmenities>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TravelPackageId, opt => opt.Ignore());

            CreateMap<TravelPackageMedia, TravelPackageMediaDTO>();
            CreateMap<TravelPackageMediaDTO, TravelPackageMedia>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TravelPackageId, opt => opt.Ignore());

            CreateMap<Reservation, ReservationDTO>();
            CreateMap<CreateReservationDTO, Reservation>();

            CreateMap<Guests, GuestsDTO>();
            CreateMap<GuestsDTO, Guests>() // mapeamento para GuestsDTO para Guests (se precisar de input)
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ReservationId, opt => opt.Ignore()); // ignora fk em mapeamento de entrada

            CreateMap<User, UserDTO>(); // Mapeamento para atualização

            CreateMap<UpdateUserDTO, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Document, opt => opt.Ignore());
        }
    }
}
