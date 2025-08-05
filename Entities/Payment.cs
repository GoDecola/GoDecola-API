using Microsoft.VisualBasic;

namespace GoDecola.API.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public string? StripePaymentIntentId { get; set; } // ID do pagamento no Stripe
        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; } // Propriedade de navegação 
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow; // Data do pagamento
        public string? Method { get; set; }
        public string? PixQrCode { get; set; }
        public string? BoletoBarcode { get; set; }
        public string? Status { get; set; }
        public long? AmountPaid { get; set; }
        public string? RedirectUrl { get; set; }
        public string? UrlVoucher { get; set; } // URL do comprovante de pagamento 
    }
}
