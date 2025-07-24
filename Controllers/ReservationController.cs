using AutoMapper;
using GoDecola.API.DTOs.ReservationDTOs;
using GoDecola.API.DTOs.TravelPackageDTOs;
using GoDecola.API.DTOs.UserDTOs;
using GoDecola.API.Entities;
using GoDecola.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoDecola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IRepository<Reservation, int> _reservationRepository;
        private readonly IRepository<User, string> _userRepository;
        private readonly IRepository<TravelPackage, int> _travelPackageRepository;
        private readonly IMapper _mapper;

        public ReservationController(
            IRepository<Reservation, int> reservationRepository,
            IRepository<User, string> userRepository,
            IRepository<TravelPackage, int> travelPackageRepository,
            IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _travelPackageRepository = travelPackageRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation(CreateReservationDTO createReservation)
        {
            // verificar se user existe
            var user = await _userRepository.GetByIdAsync(createReservation.UserId);
            if (user == null)
                return NotFound("Usuário não encontrado.");

            // verificar se travelPackage existe
            var travelPackage = await _travelPackageRepository.GetByIdAsync(createReservation.TravelPackageId);
            if (travelPackage == null)
                return NotFound("Pacoate de viagens não encontrado.");

            // mapear DTO para entidade Reservation
            var reservation = _mapper.Map<Reservation>(createReservation);
            reservation.Status = "Confirmada";
            reservation.TotalPrice = travelPackage.Price;

            await _reservationRepository.AddAsync(reservation);

            // retornar o DTO mapeado
            var response = _mapper.Map<ReservationDTO>(reservation);
            response.User = _mapper.Map<UserDTO>(user);
            response.TravelPackage = _mapper.Map<TravelPackageDTO>(travelPackage);

            return Ok(response);
        }
            

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null) return NotFound();

            var reservationResponse = _mapper.Map<ReservationDTO>(reservation);
            return Ok(reservationResponse);
        }
    }
}
