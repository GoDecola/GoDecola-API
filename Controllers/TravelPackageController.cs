using AutoMapper;
using GoDecola.API.DTOs.TravelPackageDTOs;
using GoDecola.API.Entities;
using GoDecola.API.Enums;
using GoDecola.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoDecola.API.Controllers
{
    [Route("api/travel-packages")]
    [ApiController]
    public class TravelPackageController : ControllerBase
    {
        private readonly IRepository<TravelPackage, int> _travelPackageRepository;
        private readonly IMapper _mapper;

        public TravelPackageController(IRepository<TravelPackage, int> travelPackageRepository, IMapper mapper)
        {
            _travelPackageRepository = travelPackageRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelPackageDTO>>> GetAll()
        {
            var travelPackages = await _travelPackageRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<TravelPackageDTO>>(travelPackages));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TravelPackageDTO>> GetById(int id)
        {
            var travelPackage = await _travelPackageRepository.GetByIdAsync(id);
            if (travelPackage == null)
            {
                return NotFound("Pacote de viagem não encontrado.");
            }
            return Ok(_mapper.Map<TravelPackageDTO>(travelPackage));
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create(CreateTravelPackageDTO travelPackage)
        {
            var newTravelPackage = _mapper.Map<TravelPackage>(travelPackage);

            await _travelPackageRepository.AddAsync(newTravelPackage);

            return CreatedAtAction(
                nameof(GetById), 
                new 
                { 
                    id = newTravelPackage.Id 
                }, 
                _mapper.Map<TravelPackageDTO>(newTravelPackage));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(int id, UpdateTravelPackageDTO travelPackage)
        {
            var existingTravelPackage = await _travelPackageRepository.GetByIdAsync(id);

            if (existingTravelPackage == null)
            {
                return NotFound("Pacote de viagem não encontrado.");
            }

            var updatedTravelPackage = _mapper.Map(travelPackage, existingTravelPackage);

            await _travelPackageRepository.UpdateAsync(updatedTravelPackage);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int id)
        {
            var travelPackage = await _travelPackageRepository.GetByIdAsync(id);
            if (travelPackage == null)
            {
                return NotFound("Pacote de viagem não encontrado.");
            }
            await _travelPackageRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
