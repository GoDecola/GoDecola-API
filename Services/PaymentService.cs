using GoDecola.API.Controllers;
using GoDecola.API.DTOs.PaymentDTOs;
using GoDecola.API.Entities;
using GoDecola.API.Enums;
using GoDecola.API.Repositories;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;

namespace GoDecola.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<Reservation, int> _reservationRepository;
        private readonly IRepository<Payment, int> _paymentRepository;
        private readonly IEmailService _emailService;
        private readonly StripeSettings _stripeSettings;
        private readonly ILogger<WebhookController> _logger;
        private readonly string _successUrl;
        private readonly string _cancelUrl;

        public PaymentService(
            IRepository<Reservation, int> reservationRepository,
            IRepository<Payment, int> paymentRepository,
            StripeSettings stripeSettings,
            ILogger<WebhookController> logger,
            IEmailService emailService,
            string successUrl,
            string cancelUrl)
        {
            _reservationRepository = reservationRepository;
            _paymentRepository = paymentRepository;
            _emailService = emailService;
            _stripeSettings = stripeSettings;
            _successUrl = successUrl;
            _cancelUrl = cancelUrl;
            _logger = logger;

            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        public async Task<PaymentResponseDTO> InitiateStripeCheckout(PaymentRequestDTO request)
        {
            var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId);
            if (reservation == null)
                throw new Exception("Reserva não encontrada.");

            var payment = new Payment
            {
                ReservationId = request.ReservationId,
                AmountPaid = reservation.TotalPrice,
                Method = request.Method,
                Status = PaymentStatus.PENDING.ToString(),
                PaymentDate = DateTime.UtcNow
            };

            await _paymentRepository.AddAsync(payment);

            var guestEmail = reservation.Guests?.FirstOrDefault()?.Email;

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                CustomerEmail = guestEmail,
                Metadata = new Dictionary<string, string>
                {
                    { "payment_id", payment.Id.ToString() },
                    { "reservation_id", request.ReservationId.ToString() }
                },
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
                        Quantity = 1
                    }
                },
                SuccessUrl = $"{_successUrl}?session_id={{CHECKOUT_SESSION_ID}}&payment_id={payment.Id}",
                CancelUrl = $"{_cancelUrl}?payment_id={payment.Id}"
            };

            var service = new SessionService();
            try
            {
                var session = await service.CreateAsync(options);
                payment.RedirectUrl = session.Url;
                payment.StripePaymentIntentId = session.PaymentIntentId;

                payment.Status = PaymentStatus.CONFIRMED.ToString();
                await _paymentRepository.UpdateAsync(payment);

                reservation.Status = ReservationStatus.CONFIRMED;
                await _reservationRepository.UpdateAsync(reservation);

                return new PaymentResponseDTO
                {
                    Id = payment.Id,
                    ReservationId = payment.ReservationId,
                    RedirectUrl = session.Url,
                    Status = payment.Status,
                    AmountPaid = payment.AmountPaid,
                    PaymentDate = payment.PaymentDate
                };
            }
            catch (StripeException ex)
            {
                payment.Status = PaymentStatus.FAILED.ToString();
                await _paymentRepository.UpdateAsync(payment);
                throw new Exception("Erro ao criar sessão de pagamento no Stripe: " + ex.Message);
            }
        }

        public async Task<PaymentResponseDTO> CreatePixPaymentAsync(PaymentRequestDTO request)
        {
            var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId);
            if (reservation == null)
                throw new Exception("Reserva não encontrada.");

            var pixCode = $"PIX-{Guid.NewGuid().ToString().Substring(0, 8)}";

            var payment = new Payment
            {
                ReservationId = request.ReservationId,
                AmountPaid = reservation.TotalPrice,
                Method = "PIX",
                PixQrCode = pixCode,
                Status = PaymentStatus.PENDING.ToString(),
                PaymentDate = DateTime.UtcNow
            };

            await _paymentRepository.AddAsync(payment);

            return new PaymentResponseDTO
            {
                Id = payment.Id,
                ReservationId = payment.ReservationId,
                Status = payment.Status,
                AmountPaid = payment.AmountPaid,
                PaymentDate = payment.PaymentDate,
                RedirectUrl = payment.PixQrCode,
                ReservationStatus = reservation.Status.ToString()
            };
        }

        public async Task<PaymentResponseDTO> CreateBoletoPaymentAsync(PaymentRequestDTO request)
        {
            var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId);
            if (reservation == null)
                throw new Exception("Reserva não encontrada.");

            var barcode = $"23790{new Random().Next(10000000, 99999999)}";

            var payment = new Payment
            {
                ReservationId = request.ReservationId,
                AmountPaid = reservation.TotalPrice,
                Method = "Boleto",
                BoletoBarcode = barcode,
                Status = PaymentStatus.PENDING.ToString(),
                PaymentDate = DateTime.UtcNow
            };

            await _paymentRepository.AddAsync(payment);

            return new PaymentResponseDTO
            {
                Id = payment.Id,
                ReservationId = payment.ReservationId,
                Status = payment.Status,
                AmountPaid = payment.AmountPaid,
                PaymentDate = payment.PaymentDate,
                RedirectUrl = payment.BoletoBarcode,
                ReservationStatus = reservation.Status.ToString()
            };
        }
        public async Task<IEnumerable<PaymentResponseDTO>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();

            return payments.Select(p => new PaymentResponseDTO
            {
                Id = p.Id,
                Status = p.Status,
                AmountPaid = p.AmountPaid,
                PaymentDate = p.PaymentDate,
                UrlVoucher = p.UrlVoucher,
                ReservationId = p.ReservationId,
                ReservationStatus = p.Reservation?.Status.ToString()
            });
        }

        public async Task<PaymentResponseDTO?> GetPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
                return null;

            return new PaymentResponseDTO
            {
                Id = payment.Id,
                Status = payment.Status,
                AmountPaid = payment.AmountPaid,
                PaymentDate = payment.PaymentDate,
                UrlVoucher = payment.UrlVoucher,
                ReservationId = payment.ReservationId,
                ReservationStatus = payment.Reservation?.Status.ToString()
            };
        }

        public async Task HandleStripeWebhookAsync(Event stripeEvent)
        {
            _logger.LogInformation($"[Stripe Webhook] Evento recebido: {stripeEvent.Type}");

            switch (stripeEvent.Type)
            {
                case "checkout.session.completed":
                    await HandleCheckoutSessionCompleted(stripeEvent);
                    break;
                case "payment_intent.succeeded":
                    await HandlePaymentIntentSucceeded(stripeEvent);
                    break;
                case "payment_intent.payment_failed":
                    await HandlePaymentIntentFailed(stripeEvent);
                    break;
                case "charge.refunded":
                    await HandleChargeRefunded(stripeEvent);
                    break;
            }
        }

        private async Task HandleCheckoutSessionCompleted(Event stripeEvent)
        {
            var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
            if (session == null)
            {
                _logger.LogWarning("Checkout session nula.");
                return;
            }

            Payment? payment = null;

            if (session.Metadata.TryGetValue("payment_id", out var paymentIdStr) &&
                int.TryParse(paymentIdStr, out int paymentId))
            {
                payment = await _paymentRepository.GetByIdAsync(paymentId);
            }

            if (payment == null &&
                session.Metadata.TryGetValue("reservation_id", out var reservationIdStr) &&
                int.TryParse(reservationIdStr, out int reservationId))
            {
                var allPayments = await _paymentRepository.GetAllAsync();
                payment = allPayments.FirstOrDefault(p => p.ReservationId == reservationId);
            }

            if (payment == null)
            {
                _logger.LogWarning("Nenhum pagamento encontrado para atualizar.");
                return;
            }

            payment.Status = PaymentStatus.CONFIRMED.ToString();

            var chargeService = new ChargeService();
            var charges = await chargeService.ListAsync(new ChargeListOptions
            {
                PaymentIntent = session.PaymentIntentId,
                Limit = 1
            });

            var charge = charges.Data.FirstOrDefault();
            if (charge != null)
            {
                payment.UrlVoucher = charge.ReceiptUrl;
            }

            await _paymentRepository.UpdateAsync(payment);
            var reservation = await _reservationRepository.GetByIdAsync(payment.ReservationId);
            var guest = reservation!.Guests!.FirstOrDefault();

            if (guest != null)
            {
                await _emailService.SendPaymentVoucherAsync(
                    guest.Email,
                    guest.FirstName,
                    payment.UrlVoucher,
                    payment.AmountPaid
                );
            }

            if (reservation != null && reservation.Status == ReservationStatus.PENDING)
            {
                reservation.Status = ReservationStatus.CONFIRMED;
                await _reservationRepository.UpdateAsync(reservation);
            }

        }

        private async Task HandlePaymentIntentSucceeded(Event stripeEvent)
        {
            var intent = stripeEvent.Data.Object as PaymentIntent;
            if (intent == null) return;

            Payment? payment = null;

            if (intent.Metadata.TryGetValue("payment_id", out var paymentIdStr) &&
                int.TryParse(paymentIdStr, out int paymentId))
            {
                payment = await _paymentRepository.GetByIdAsync(paymentId);
            }

            if (payment == null && !string.IsNullOrEmpty(intent.Id))
            {
                var allPayments = await _paymentRepository.GetAllAsync();
                payment = allPayments.FirstOrDefault(p => p.StripePaymentIntentId == intent.Id);
            }

            if (payment == null) return;

            payment.Status = PaymentStatus.CONFIRMED.ToString();
            await _paymentRepository.UpdateAsync(payment);


            var reservation = await _reservationRepository.GetByIdAsync(payment.ReservationId);
            if (reservation != null && reservation.Status == ReservationStatus.PENDING)
            {
                reservation.Status = ReservationStatus.CONFIRMED;
                await _reservationRepository.UpdateAsync(reservation);

            }
        }

        private async Task HandlePaymentIntentFailed(Event stripeEvent)
        {
            var intent = stripeEvent.Data.Object as PaymentIntent;
            if (intent == null) return;

            Payment? payment = null;

            if (intent.Metadata.TryGetValue("payment_id", out var paymentIdStr) &&
                int.TryParse(paymentIdStr, out int paymentId))
            {
                payment = await _paymentRepository.GetByIdAsync(paymentId);
            }

            if (payment == null && !string.IsNullOrEmpty(intent.Id))
            {
                var allPayments = await _paymentRepository.GetAllAsync();
                payment = allPayments.FirstOrDefault(p => p.StripePaymentIntentId == intent.Id);
            }

            if (payment == null) return;

            payment.Status = PaymentStatus.FAILED.ToString();
            await _paymentRepository.UpdateAsync(payment);


            var reservation = await _reservationRepository.GetByIdAsync(payment.ReservationId);
            if (reservation != null)
            {
                reservation.Status = ReservationStatus.CANCELLED;
                await _reservationRepository.UpdateAsync(reservation);

            }
        }

        private async Task HandleChargeRefunded(Event stripeEvent)
        {
            var charge = stripeEvent.Data.Object as Charge;
            if (charge == null) return;

            Payment? payment = null;

            if (charge.Metadata.TryGetValue("payment_id", out var paymentIdStr) &&
                int.TryParse(paymentIdStr, out int paymentId))
            {
                payment = await _paymentRepository.GetByIdAsync(paymentId);
            }

            if (payment == null && !string.IsNullOrEmpty(charge.PaymentIntentId))
            {
                var allPayments = await _paymentRepository.GetAllAsync();
                payment = allPayments.FirstOrDefault(p => p.StripePaymentIntentId == charge.PaymentIntentId);
            }

            if (payment == null) return;

            payment.Status = PaymentStatus.CANCELLED.ToString();
            await _paymentRepository.UpdateAsync(payment);

            var reservation = await _reservationRepository.GetByIdAsync(payment.ReservationId);
            if (reservation != null)
            {
                reservation.Status = ReservationStatus.CANCELLED;
                await _reservationRepository.UpdateAsync(reservation);
            }
        }
        public async Task UpdatePaymentStatusAsync(int paymentId, string newStatus) // pro admin alterar manualmente
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null) throw new Exception("Pagamento não encontrado.");

            payment.Status = newStatus;

            if (newStatus == PaymentStatus.CONFIRMED.ToString() &&
                (payment.Method == "PIX" || payment.Method == "Boleto"))
            {
                payment.UrlVoucher = $"https://godecola.com/voucher/{payment.Id}"; //envio comprovante apenas quando pix e boleto
                await _paymentRepository.UpdateAsync(payment);

                var reservation = await _reservationRepository.GetByIdAsync(payment.ReservationId);
                if (reservation != null && reservation.Status == ReservationStatus.PENDING)
                {
                    reservation.Status = ReservationStatus.CONFIRMED;
                    await _reservationRepository.UpdateAsync(reservation);
                }

                var guest = reservation!.Guests!.FirstOrDefault();
                if (guest != null)
                {
                    await _emailService.SendPaymentVoucherAsync(
                        guest.Email,
                        guest.FirstName,
                        payment.UrlVoucher,
                        payment.AmountPaid
                    );
                }
                return;
            }
            else if (newStatus == PaymentStatus.CANCELLED.ToString()) //para pagamentos cancelados
            {
                await _paymentRepository.UpdateAsync(payment);

                var reservation = await _reservationRepository.GetByIdAsync(payment.ReservationId);
                if (reservation != null)
                {
                    reservation.Status = ReservationStatus.CANCELLED;
                    await _reservationRepository.UpdateAsync(reservation);
                }
                return;
            }
            else if (newStatus == PaymentStatus.FAILED.ToString()) // pagamentos que deram falhas (mas pode tentar pagar com outro metodo
            {
                // Só atualiza o pagamento, reserva fica pendente
                await _paymentRepository.UpdateAsync(payment);
                return;
            }

            await _paymentRepository.UpdateAsync(payment);
        }



    }
}