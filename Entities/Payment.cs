using Microsoft.VisualBasic;

namespace GoDecola.API.Entities.payment
{
    public class Paymentcs
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }
        public float AmountPaid { get; set; }
        public string UrlVoucher { get; set; } // URL do comprovante de pagamento
        public Reservation? Reservation { get; set; } // Propriedade de navegação 
    }
}
