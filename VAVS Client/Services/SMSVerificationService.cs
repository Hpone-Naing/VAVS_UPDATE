namespace VAVS_Client.Services
{
    public interface SMSVerificationService
    {
        public Task<string> SendSMSOTP(string phoneNumber, string msg);
    }
}
