namespace GoDecola.API.Services
{
    public interface IEmailService
    {
        Task SendForgotPasswordEmailAsync(string toEmail, string userName, string resetLink);
        Task SendPaymentVoucherAsync(string to, string guestName, string voucherUrl, long? amountPaid);

    }
}
