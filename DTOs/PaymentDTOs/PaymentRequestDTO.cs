namespace GoDecola.API.DTOs.PaymentDTOs
{
    public class PaymentRequestDTO
    {
        public int ReservationId { get; set; }  // id da reserva associada ao pagamento
        public float Amount { get; set; }  // valor do pagamento
        public string Method { get; set; } = string.Empty; // método de pagamento (cartão de crédito, pix, etc.))
    }
}
