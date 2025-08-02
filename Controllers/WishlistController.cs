using AutoMapper;
using GoDecola.API.DTOs.Wishlist;
using GoDecola.API.Entities;
using GoDecola.API.Enums;
using GoDecola.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoDecola.API.Controllers
{
    [Authorize]
    [Route("api/wishlist")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistRepository _wishlistRepo;
        private readonly IRepository<TravelPackage, int> _packageRepo;
        private readonly IMapper _mapper;

        public WishlistController(
            IWishlistRepository wishlistRepo,
            IRepository<TravelPackage, int> packageRepo,
            IMapper mapper)
        {
            _wishlistRepo = wishlistRepo;
            _packageRepo = packageRepo;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromBody] CreateWishlistDTO addWishlist)
        {
            // autenticador jwt para adicionar um pacote de viagem a wishlist do usuario logado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // busca o pacote de viagem pelo id no banco de daods
            var travelPackage = await _packageRepo.GetByIdAsync(addWishlist.TravelPackageId);
            if (travelPackage == null)
                return NotFound("Pacote de viagem não encontrado.");

            // verifica se o pacote ja está na wishlist do usuario
            var existing = await _wishlistRepo.GetByUserAndPackageAsync(userId, addWishlist.TravelPackageId);
            if (existing != null)
                return Conflict("Este pacote já está na sua wishlist.");

            // cr um novo item de wishlist
            var wishlistItem = new Wishlist
            {
                UserId = userId,
                TravelPackageId = addWishlist.TravelPackageId,
                AddedDate = DateTime.UtcNow
            };

            // salva e retorna 200 ok
            await _wishlistRepo.AddAsync(wishlistItem);
            return Ok("Pacote adicionado à wishlist com sucesso.");
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            // autenticador jwt para listar a wishlist do usuario logado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // busca todos os itens da wishlist do usuario
            var items = await _wishlistRepo.GetAllByUserAsync(userId);

            // mapeia os itens para a dto 
            var result = items.Select(item => new WishlistItemDTO
            {
                TravelPackageId = item.TravelPackageId,
                Title = item.TravelPackage?.Title ?? "Indisponível", // se o pacote nao existir, retorna "Indisponível"
                Description = item.TravelPackage?.Description,
                IsAvailable = item.TravelPackage?.IsActive ?? false, // verifica se o pacote esta ativo
                AddedDate = item.AddedDate // data em que foi adicionado a wishlist
            });

            return Ok(result);
        }
        // TODO: DEBATER SE APEAS O USUARIO PODE REMOVER DA WISHLIST OU O ADMINISTRADOR TAMBEM PODE
        [Authorize(Roles = nameof(UserType.USER))]
        [HttpDelete("{travelPackageId}")]
        public async Task<IActionResult> RemoveFromWishlist(int travelPackageId)
        {
            // autenticador jwt para remover um pacote de viagem da wishlist do usuario logado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // busca o item da wishlist pelo id do usuario e do pacote de viagem
            var item = await _wishlistRepo.GetByUserAndPackageAsync(userId, travelPackageId);
            if (item == null)
                return NotFound("Item da wishlist não encontrado.");

            await _wishlistRepo.DeleteAsync(item.Id);
            return Ok("Item removido da wishlist.");
        }
    }
}
