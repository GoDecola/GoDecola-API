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
