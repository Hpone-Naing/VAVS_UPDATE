using VAVS_Client.Util;

namespace VAVS_Client.Services.Impl
{
    public class SMSVerificationServiceImpl : SMSVerificationService
    {
        private readonly HttpClient _httpClient;

        public SMSVerificationServiceImpl(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> SendSMSOTP(string phoneNumber, string msg)
        {
            string apiKey = Utility.SMSPOH_API_KEY;
            string message = msg;
            string baseUrl = "https://smspoh.com/api/http/send";
            string url = $"{baseUrl}?key={apiKey}&message={message}&recipients={phoneNumber}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success: " + response.StatusCode);
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                Console.WriteLine($"Failed to send message. Status code: {response.StatusCode}");
                throw new HttpRequestException($"Failed to send message. Status code: {response.StatusCode}");
            }
        }
    }
}
