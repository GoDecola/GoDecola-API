using AutoMapper;
using GoDecola.API.DTOs.PaymentDTOs;
using GoDecola.API.DTOs.UserDTOs;
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
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] PaymentRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _paymentService.InitiateStripeCheckout(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize(Roles = "ADMIN,SUPPORT")]
        [HttpGet("admin/list")]
        public async Task<IActionResult> GetAllPayments()
        {
            var response = await _paymentService.GetAllPaymentsAsync();
            return Ok(response);
        }

        [Authorize(Roles = "ADMIN,SUPPORT")]
        [HttpGet("admin/{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var response = await _paymentService.GetPaymentByIdAsync(id);
            if (response == null)
                return NotFound(new { message = "Pagamento não encontrado." });

            return Ok(response);
        }

    }
}
