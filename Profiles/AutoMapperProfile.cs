using AutoMapper;
using GoDecola.API.Entities;
using GoDecola.API.DTOs;
using GoDecola.API.DTOs.UserDTOs;
using GoDecola.API.DTOs.TravelPackageDTOs;
using GoDecola.API.DTOs.ReservationDTOs;
using GoDecola.API.DTOs.PaymentDTOs;

namespace GoDecola.API.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // ------------------------ USER ---------------------------------

            CreateMap<User, UserDTO>();
            CreateMap<CreateUserDTO, User>();
            CreateMap<UpdateUserDTO, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // ------------------------ TRAVEL PACKAGE -------------------------

            CreateMap<TravelPackage, TravelPackageDTO>()
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Reviews.Any() ? Math.Round(src.Reviews.Average(r => r.Rating), 1) : (double?)null))
                .ForMember(dest => dest.IsCurrentlyOnPromotion, opt => opt.MapFrom(src => src.IsCurrentlyOnPromotion)) // mapeia a propriedade IsCurrentlyOnPromotion do TravelPackage para IsCurrentylOnPromotion do TravelPackageDTO
                .ForMember(dest => dest.MediasUrl, opt => opt.MapFrom(src => src.Medias)); // mapeia a collection de TravelPackageMedia para TravelPackageMediaDTO
            CreateMap<CreateTravelPackageDTO, TravelPackage>();
            CreateMap<UpdateTravelPackageDTO, TravelPackage>(); 

            CreateMap<AccommodationDetails, AccommodationDTO>();
            CreateMap<AccommodationDTO, AccommodationDetails>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TravelPackageId, opt => opt.Ignore());

            CreateMap<Address, AddressDTO>();
            CreateMap<AddressDTO, Address>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<TravelPackageMedia, TravelPackageMediaDTO>();
            CreateMap<TravelPackageMediaDTO, TravelPackageMedia>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TravelPackageId, opt => opt.Ignore());

            // ------------------------ RESERVATION -------------------------

            CreateMap<Reservation, ReservationDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CreateReservationDTO, Reservation>();
            CreateMap<Guests, GuestsDTO>();
            CreateMap<GuestsDTO, Guests>() // mapeamento para GuestsDTO para Guests (se precisar de input)
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ReservationId, opt => opt.Ignore()); // ignora fk em mapeamento de entrada


            // ------------------------ PAYMENT ------------------------------

            CreateMap<Payment, PaymentRequestDTO>(); 
            CreateMap<PaymentRequestDTO, Payment>();

            CreateMap<Payment, PaymentResponseDTO>()
                .ForMember(dest => dest.RedirectUrl, opt => opt.MapFrom(src => src.RedirectUrl))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)) 
                .ForMember(dest => dest.AmountPaid, opt => opt.MapFrom(src => src.AmountPaid)) 
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate)); 
        }
    }
}
