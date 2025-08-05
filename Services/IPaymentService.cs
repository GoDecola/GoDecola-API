using GoDecola.API.DTOs.PaymentDTOs;
using GoDecola.API.Entities;
using Stripe;
using System.Threading.Tasks;

namespace GoDecola.API.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponseDTO> InitiateStripeCheckout(PaymentRequestDTO request);
        Task HandleStripeWebhookAsync(Event stripeEvent);
        Task<IEnumerable<PaymentResponseDTO>> GetAllPaymentsAsync();
        Task<PaymentResponseDTO?> GetPaymentByIdAsync(int id);
        Task<PaymentResponseDTO> CreatePixPaymentAsync(PaymentRequestDTO request);
        Task<PaymentResponseDTO> CreateBoletoPaymentAsync(PaymentRequestDTO request);
        Task UpdatePaymentStatusAsync(int paymentId, string newStatus);

    }
}
