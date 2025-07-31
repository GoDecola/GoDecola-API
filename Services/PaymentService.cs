using GoDecola.API.DTOs.PaymentDTOs;
using GoDecola.API.Entities;
using GoDecola.API.Enums;
using GoDecola.API.Repositories;
using Stripe;
using Stripe.Checkout;

namespace GoDecola.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<Reservation, int> _reservationRepository;
        private readonly IRepository<Payment, int> _paymentRepository;
        private readonly StripeSettings _stripeSettings;
        private readonly string _successUrl;
        private readonly string _cancelUrl;

        public PaymentService(
            IRepository<Reservation, int> reservationRepository,
            IRepository<Payment, int> paymentRepository,
            StripeSettings stripeSettings,
            string successUrl,
            string cancelUrl)
        {
            _reservationRepository = reservationRepository;
            _paymentRepository = paymentRepository;
            _stripeSettings = stripeSettings;
            _successUrl = successUrl;
            _cancelUrl = cancelUrl;

            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        public async Task<PaymentResponseDTO> InitiateStripeCheckout(PaymentRequestDTO request)
        {
            var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId);

            var payment = new Payment
            {
                ReservationId = request.ReservationId,
                AmountPaid = reservation.TotalPrice,
                Method = request.Method,
                Status = PaymentStatus.PENDING.ToString(),
                PaymentDate = DateTime.UtcNow
            };

            await _paymentRepository.AddAsync(payment);

            var option = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "brl",
                            UnitAmount = reservation.TotalPrice, 
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Reserva #{reservation.Id}",
                                Description = $"Reserva ID: {request.ReservationId}"
                            }
                        },
                        Quantity = 1,
                    }
                },
                SuccessUrl = _successUrl + $"?session_id={{CHEKOUT_SESSION_ID}}&payment_id={payment.Id}",
                CancelUrl = _cancelUrl + $"?payment_id={payment.Id}",
                Metadata  = new Dictionary<string, string>
                {
                    { "payment_id", payment.Id.ToString() },
                    { "reservation_id", request.ReservationId.ToString() }
                }

            };

            var service = new SessionService();
            Session session;
            try 
            { 
                session = await service.CreateAsync(option);
            }
            catch (StripeException ex)
            {
                payment.Status = PaymentStatus.FAILED.ToString();
                await _paymentRepository.UpdateAsync(payment);
                throw new Exception("Erro ao criar sessão de pagamento no Stripe: " + ex.Message);
            }

            payment.RedirectUrl = session.Url;
            payment.StripePaymentIntentId = session.PaymentIntentId;
            await _paymentRepository.UpdateAsync(payment);

            return new PaymentResponseDTO
            {
                RedirectUrl = session.Url,
                Status = payment.Status,
                AmountPaid = payment.AmountPaid,
                PaymentDate = payment.PaymentDate
            };
        }

    }
}
