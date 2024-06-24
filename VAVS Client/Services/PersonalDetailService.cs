using VAVS_Client.Classes;

namespace VAVS_Client.Services
{
    public interface PersonalDetailService
    {
        PersonalDetail FindPersonalDetailByNrc(string nrc);
        bool CreatePersonalDetail(PersonalDetail personalDetail);
        PersonalDetail FindPersonalDetailByPhoneNumber(string phoneNumber);
        Task<PersonalDetail> GetPersonalInformationByNRC(string nrc);
        Task<PersonalDetail> GetPersonalInformationByNRCInDBAndAPI(string nrc);
        Task<PersonalDetail> GetPersonalInformationByPhoneNumber(string phoneNumber);
        Task<PersonalDetail> GetPersonalInformationByPhoneNumberInDBAndAPI(string phoneNumber);
        Task<bool> ResetPhoneNumber(ResetPhonenumber resetPhonenumber);
        List<string> GetNRCTownshipInitials(string nrcTownshipNumber);
       // Township GetTownshipByName(string townshipName);
    }
}
