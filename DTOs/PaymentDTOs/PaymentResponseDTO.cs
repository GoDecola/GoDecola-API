namespace GoDecola.API.DTOs.PaymentDTOs
{
    public class PaymentResponseDTO
    {
        public int Id { get; set; }
        public string RedirectUrl { get; set; } = string.Empty; //url de redirecionamento para o pagamento
        public string? Status { get; set; } // status do pagamento (pendente, confirmado, falha, cancelado)
        public long? AmountPaid { get; set; } // valor pago
        public DateTime? PaymentDate { get; set; } // data do pagamentoo
        public string? UrlVoucher { get; set; }     // URL do comprovante
        public int ReservationId { get; set; }      // Id da reserva vinculada
        public string? ReservationStatus { get; set; } // Status da reserva
    }
}
