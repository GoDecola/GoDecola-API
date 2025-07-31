using GoDecola.API.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace GoDecola.API.Controllers
{
    [ApiController]
    [Route("api/stripe-webhook")]
    public class WebhookController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(
            IPaymentService paymentService,
            IConfiguration configuration,
            ILogger<WebhookController> logger)
        {
            _paymentService = paymentService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var webhookSecret = _configuration["Stripe:WebhookSecret"];

                if (string.IsNullOrEmpty(webhookSecret))
                {            
                    return StatusCode(500, new { error = "Webhook Secret não configurado" });
                }

                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    webhookSecret
                );
                //apagar dps, apenas para testes
                _logger.LogInformation($"Evento Stripe recebido: {stripeEvent.Type}");

                await _paymentService.HandleStripeWebhookAsync(stripeEvent);
                
                return Ok();
            }
            catch (StripeException e)
            {
                
                return BadRequest(new { error = e.Message });
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno no Webhook" });
            }
        }
    }
}

