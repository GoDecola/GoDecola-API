namespace GoDecola.API.DTOs.PaymentDTOs
{
    public class PaymentResponseDTO
    {
        public string RedirectUrl { get; set; } = string.Empty; //url de redirecionamento para o pagamento
        public string? Status { get; set; } // status do pagamento (pendente, confirmado, falha, cancelado)
        public float? AmountPaid { get; set; } // valor pago
        public DateTime? PaymentDate { get; set; } // data do pagamentoo
    }
}
