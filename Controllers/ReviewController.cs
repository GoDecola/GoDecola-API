using AutoMapper;
using GoDecola.API.DTOs.ReviewDTOs;
using GoDecola.API.Entities;
using GoDecola.API.Enums;
using GoDecola.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoDecola.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ReviewController(IReviewRepository reviewRepository, IReservationRepository reservationRepository, IMapper mapper, UserManager<User> userManager)
        {
            _reviewRepository = reviewRepository;
            _reservationRepository = reservationRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPatch("admin/reviews/{reviewId}/status")]
        [Authorize(Roles = nameof(UserType.ADMIN))]
        public async Task<IActionResult> UpdateReviewStatus(int reviewId, StatusReviewDTO statusReview)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            if (review == null)
            {
                return NotFound($"Review {reviewId} não encontrada");
            }

            review.Status = statusReview.Status.ToString();

            await _reviewRepository.UpdateAsync(review);

            return NoContent();
        }

        [HttpGet("admin/reviews")]
        [Authorize(Roles = nameof(UserType.ADMIN))]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetAllReviews()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            if (reviews == null || !reviews.Any())
            {
                return Ok(new List<ReviewDTO>()); // retorna lista vazia se nao houver reviews
            }
            return Ok(_mapper.Map<IEnumerable<ReviewDTO>>(reviews));
        }

        [HttpGet("travel-packages/{packageId}/reviews")]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetReviewsByPackage(int packageId)
        {
            var reviews = await _reviewRepository.GetByPackageIdAsync(packageId);
            if (reviews == null || !reviews.Any())
            {
                return Ok(new List<ReviewDTO>()); // retorna lista vazia se nao houver reviews
            }

            return Ok(_mapper.Map<IEnumerable<ReviewDTO>>(reviews));
        }

        [HttpPost("{travelPackageId}/reviews")]
        [Authorize]
        public async Task<IActionResult> CreateReview(CreateReviewDTO createReview)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            if (userId == null)
            {
                return Unauthorized("Usuário não autenticado.");
            }

            var existingReview = await _reviewRepository.FindOneAsync(r => r.UserId == userId && r.TravelPackageId == createReview.TravelPackageId);
            if (existingReview != null)
            {
                return BadRequest("Você já avaliou este pacote de viagem.");
            }

            var reservation = await _reservationRepository.FindOneWithDetailsAsync(r => r.UserId == userId && r.TravelPackageId == createReview.TravelPackageId);

            if (reservation == null)
            {
                return Forbid("Apenas usuários que reservaram este pacote podem avaliá-lo.");
            }

            if (reservation.TravelPackage.EndDate > DateTime.UtcNow)
            {
                return Forbid("Você só pode avaliar a viagem após a data de término.");
            }

            var review = _mapper.Map<Review>(createReview);
            review.UserId = userId;
            review.ReviewDate = DateTime.UtcNow;
            review.Status = ReviewStatus.PENDING.ToString();

            var newReview = await _reviewRepository.AddAsync(review);
            var reviewDto = _mapper.Map<ReviewDTO>(newReview);

            return CreatedAtAction(nameof(GetReviewsByPackage), new { packageId = newReview.TravelPackageId }, reviewDto);
        }

        [HttpGet("users/me/reviews")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetMyReviews()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var reviews = await _reviewRepository.GetByUserIdAsync(userId);
            return Ok(_mapper.Map<IEnumerable<ReviewDTO>>(reviews));
        }
    }
}
