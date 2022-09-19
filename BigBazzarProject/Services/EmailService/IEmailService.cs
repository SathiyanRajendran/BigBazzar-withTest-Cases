namespace BigBazzar.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO request);
    }
}
