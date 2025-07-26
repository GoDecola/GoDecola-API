using GoDecola.API.DTOs.PaymentDTOs;

namespace GoDecola.API.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponseDTO> InitiateStripeCheckout(PaymentRequestDTO request);
    }
}
