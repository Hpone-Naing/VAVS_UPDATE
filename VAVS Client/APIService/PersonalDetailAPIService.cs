using VAVS_Client.Classes;

namespace VAVS_Client.APIService
{
    public interface PersonalDetailAPIService
    {
        Task<PersonalDetail> GetPersonalInformationByNRC(string nrc);
        Task<PersonalDetail> GetPersonalInformationByPhoneNumber(string phoneNumber);
        Task<bool> ResetPhoneNumber(string nrc, string oldPhoneNumber, string newPhoneNumber);
        Task<List<string>> GetNrcTownshipInitials(string nrcTownshipNumber);

    }
}
