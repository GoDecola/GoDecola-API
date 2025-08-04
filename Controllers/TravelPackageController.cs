using AutoMapper;
using GoDecola.API.DTOs.TravelPackageDTOs;
using GoDecola.API.Entities;
using GoDecola.API.Enums;
using GoDecola.API.Repositories;
using GoDecola.API.Services;
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
        private readonly IMediaService _mediaService;
        private readonly IRepository<TravelPackage, int> _travelPackageRepository;
        private readonly IMapper _mapper;

        public TravelPackageController(IRepository<TravelPackage, int> travelPackageRepository, IMapper mapper, IMediaService mediaService)
        {
            _travelPackageRepository = travelPackageRepository;
            _mapper = mapper;
            _mediaService = mediaService;
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<TravelPackageDTO>>> SearchPackages(
    [FromQuery] string? destination,
    [FromQuery] long? minPrice,
    [FromQuery] long? maxPrice,
    [FromQuery] DateTime? startDate,
    [FromQuery] DateTime? endDate,
    [FromQuery] int? numberGuests)
        {
            var allPackages = await _travelPackageRepository.GetAllAsync();
            Console.WriteLine($"Initial packages count: {allPackages.Count()}");
            Console.WriteLine($"Received: destination={destination}, minPrice={minPrice}, maxPrice={maxPrice}, startDate={startDate:yyyy-MM-dd}, endDate={endDate:yyyy-MM-dd}, numberGuests={numberGuests}");

            // Filtro por destino
            if (!string.IsNullOrWhiteSpace(destination))
            {
                var searchTerm = destination.Trim().ToLower();
                allPackages = allPackages.Where(p =>
                    (p.Title?.ToLower().Contains(searchTerm) == true) ||
                    (p.Destination?.ToLower().Contains(searchTerm) == true) ||                    
                    (p.AccommodationDetails?.Address?.City?.ToLower().Contains(searchTerm) == true) ||
                    (p.AccommodationDetails?.Address?.State?.ToLower().Contains(searchTerm) == true)
                );
                Console.WriteLine($"After destination filter: {allPackages.Count()}");
            }

            // Filtro por número de hóspedes
            if (numberGuests.HasValue)
            {
                allPackages = allPackages.Where(p => p.NumberGuests >= numberGuests.Value);
                Console.WriteLine($"After numberGuests filter: {allPackages.Count()}");
            }

            // Filtro por preço
            if (minPrice.HasValue)
            {
                allPackages = allPackages.Where(p => p.Price >= minPrice.Value);
                Console.WriteLine($"After minPrice filter: {allPackages.Count()}");
            }
            if (maxPrice.HasValue)
            {
                allPackages = allPackages.Where(p => p.Price <= maxPrice.Value);
                Console.WriteLine($"After maxPrice filter: {allPackages.Count()}");
            }

            // Filtro por intervalo de datas
            if (startDate.HasValue && endDate.HasValue)
            {
                // Garantir que as datas sejam tratadas como UTC para consistência
                var startDateUtc = startDate.Value.Date;
                var endDateUtc = endDate.Value.Date;
                allPackages = allPackages.Where(p =>
                    p.StartDate.Date <= startDateUtc &&
                    p.EndDate.Date >= endDateUtc);
                Console.WriteLine($"After date interval filter (startDate={startDateUtc:yyyy-MM-dd}, endDate={endDateUtc:yyyy-MM-dd}): {allPackages.Count()}");
            }
            else if (startDate.HasValue)
            {
                var startDateUtc = startDate.Value.Date;
                allPackages = allPackages.Where(p => p.StartDate.Date <= startDateUtc);
                Console.WriteLine($"After startDate filter (startDate={startDateUtc:yyyy-MM-dd}): {allPackages.Count()}");
            }
            else if (endDate.HasValue)
            {
                var endDateUtc = endDate.Value.Date;
                allPackages = allPackages.Where(p => p.EndDate.Date >= endDateUtc);
                Console.WriteLine($"After endDate filter (endDate={endDateUtc:yyyy-MM-dd}): {allPackages.Count()}");
            }

            // Log dos pacotes retornados para depuração
            foreach (var package in allPackages)
            {
                Console.WriteLine($"Package ID={package.Id}, StartDate={package.StartDate:yyyy-MM-dd}, EndDate={package.EndDate:yyyy-MM-dd}");
            }

            return Ok(_mapper.Map<IEnumerable<TravelPackageDTO>>(allPackages));
        }


        [HttpPost("{id}/media")]
        [Authorize (Roles = nameof(UserType.ADMIN))]
        public async Task<IActionResult> UploadMedia(int id, [FromForm] List<IFormFile> files)
        {
            var travelPackage = await _travelPackageRepository.GetByIdAsync(id);
            if (travelPackage == null)
            {
                return NotFound("Pacote de viagem não encontrado.");
            }

            if (files == null || files.Count == 0)
            {
                return BadRequest("Nenhum arquivo enviado.");
            }

            var uploadedMediasEntities = new List<TravelPackageMedia>();

            foreach (var file in files) 
            {
                try 
                {
                    var newMedia = await _mediaService.UploadMediaForTravelPackageAsync(file, id);
                    uploadedMediasEntities.Add(newMedia);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest($"Erro ao fazer upload do arquivo {file.FileName}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao fazer upload do arquivo {file.FileName}: {ex.Message}");
                }
            }

            var uploadedMediasDto = _mapper.Map<List<TravelPackageMediaDTO>>(uploadedMediasEntities); // mapeia as entidades para DTOs

            return CreatedAtAction(nameof(UploadMedia), new { id = id }, uploadedMediasDto); 
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
        [Authorize(Roles = nameof(UserType.ADMIN))]
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
        [Authorize(Roles = nameof(UserType.ADMIN))]
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
        [Authorize(Roles = nameof(UserType.ADMIN))]
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
