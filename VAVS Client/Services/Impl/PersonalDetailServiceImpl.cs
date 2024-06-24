using VAVS_Client.Data;
using VAVS_Client.Util;
using VAVS_Client.APIService;
using VAVS_Client.Classes;
using System.Web;
using VAVS_Client.Factories;

namespace VAVS_Client.Services.Impl
{
    public class PersonalDetailServiceImpl : AbstractServiceImpl<PersonalDetail>, PersonalDetailService
    {
        private readonly ILogger<PersonalDetailServiceImpl> _logger;
        private readonly FileService _fileService;
        private readonly HttpClient _httpClient;
        private readonly PersonalDetailAPIService _personalDetailAPIService;
        private readonly TaxValidationService _taxValidationService;

        public PersonalDetailServiceImpl(VAVSClientDBContext context, HttpClient httpClient, FileService fileService, ILogger<PersonalDetailServiceImpl> logger, PersonalDetailAPIService personalDetailAPIService, TaxValidationService taxValidationService) : base(context, logger)
        {
            _httpClient = httpClient;
            _fileService = fileService;
            _logger = logger;
            _personalDetailAPIService = personalDetailAPIService;
            _taxValidationService = taxValidationService;
        }

        public bool CreatePersonalDetail(PersonalDetail personalDetail)
        {
            _logger.LogInformation(">>>>>>>>>> [PersonDetailServiceImpl][CreatePersonDetail] Create PersonDetail. <<<<<<<<<<");
            try
            {
                personalDetail.IsDeleted = false;
                personalDetail.RegistrationStatus = "Pending";
                if(personalDetail.NRCBackImagePath == null)
                {
                    personalDetail.NRCBackImagePath = "default.jpg";
                }
                if (personalDetail.NRCFrontImagePath == null)
                {
                    personalDetail.NRCFrontImagePath = "default.jpg";
                }
                if (!personalDetail.NRCTownshipNumber.EndsWith("/"))
                {
                    personalDetail.NRCTownshipNumber = string.Concat(personalDetail.NRCTownshipNumber, "/");
                }
                //personalDetail.NRCTownshipNumber = string.Concat(personalDetail.NRCTownshipNumber, "/");
                Township township = _context.Townships.FirstOrDefault(township => township.TownshipPkid == personalDetail.TownshipPkid);
                personalDetail.Township = township;
                StateDivision stateDivision = _context.StateDivisions.FirstOrDefault(stateDivision => stateDivision.StateDivisionCode == (personalDetail.StateDivisionPkid <=9 ? "0"+personalDetail.StateDivisionPkid : personalDetail.StateDivisionPkid.ToString()));
                
                personalDetail.StateDivision = stateDivision;
                personalDetail.EntryDate = DateTime.Now;
                personalDetail.PhoneNumber = personalDetail.MakePhoneNumberWithCountryCode();
                personalDetail.CreatedBy = 1;
                personalDetail.CreatedDate = DateTime.Now;
                _logger.LogInformation($">>>>>>>>>> Success. Create PersonDetail. <<<<<<<<<<");
                
                return Create(personalDetail);
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when creating PersonDetail. <<<<<<<<<<" + e);
                throw;
            }
        }

        public PersonalDetail FindPersonalDetailByNrc(string nrc)
        {
            Console.WriteLine("service nrc: " + nrc);
            Console.WriteLine("here FindPersonalDetailByNrc...................");
            _logger.LogInformation(">>>>>>>>>> [PersonDetailServiceImpl][FindPersonDetailByNrc] Find person by Nrc. <<<<<<<<<<");
            try
            {
                string NRCTownshipNumber = nrc.Split(";")[0];
                string NRCTownshipInitial = nrc.Split(";")[1];
                string NRCType = nrc.Split(";")[2];
                string NRCNumber = nrc.Split(";")[3];
                Console.WriteLine("NRCTownshipNumber..........." + NRCTownshipNumber);
                Console.WriteLine("NRCTownshipInitial..........." + NRCTownshipInitial);
                Console.WriteLine("NRCType..........." + NRCType);
                Console.WriteLine("NRCNumber..........." + NRCNumber);
                PersonalDetail personalDetail = _context.PersonalDetails
                    .Where(personalDetil =>
                        personalDetil.IsDeleted == false &&
                        personalDetil.NRCTownshipNumber == NRCTownshipNumber &&
                        personalDetil.NRCTownshipInitial== NRCTownshipInitial &&
                        personalDetil.NRCType == NRCType &&
                        personalDetil.NRCNumber == NRCNumber).Include(personalDetail => personalDetail.Township).Include(personalDetail => personalDetail.Township.StateDivision).FirstOrDefault();
                _logger.LogInformation($">>>>>>>>>> Success. Find person by Nrc. <<<<<<<<<<");
                Console.WriteLine("personal Detail == null? " + (personalDetail == null));
                return personalDetail;
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding person by Nrc. <<<<<<<<<<" + e);
                throw;
            }
        }

        public PersonalDetail FindPersonalDetailByPhoneNumber(string phoneNumber)
        {
            _logger.LogInformation(">>>>>>>>>> [PersonDetailServiceImpl][FindPersonalDetailByPhoneNumber] Find person by phone number. <<<<<<<<<<");
            try
            {
                PersonalDetail personalDetail = _context.PersonalDetails
                    .FirstOrDefault(personalDetil =>
                        personalDetil.IsDeleted == false &&
                        personalDetil.PhoneNumber == Utility.MakePhoneNumberWithCountryCode(phoneNumber)
                        );
                _logger.LogInformation($">>>>>>>>>> Success. Find person by phone number. <<<<<<<<<<");
                return personalDetail;
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding person by phone number. <<<<<<<<<<" + e);
                throw;
            }
        }

        public async Task<PersonalDetail> GetPersonalInformationByNRC(string nrc)
        {
            _logger.LogInformation(">>>>>>>>>> [PersonDetailServiceImpl][GetPersonalInformationByNRC] Get personal information by nrc. <<<<<<<<<<");
            try
            {
                //PersonalInformation personalInfo = await _personalDetailAPIService.GetPersonalInformationByNRC(nrc);
                PersonalDetail personalInfo = await _personalDetailAPIService.GetPersonalInformationByNRC(nrc);
                return personalInfo;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding person by phone number. <<<<<<<<<<" + e);
                throw new HttpRequestException($"Failed to send message. Status code: {e.StatusCode}");
            }
        }

        public async Task<PersonalDetail> GetPersonalInformationByNRCInDBAndAPI(string nrc)
        {
            try
            {
                Console.WriteLine("Nrc /...................." + nrc);

                string nrcConcatWithSemiComa = Utility.ConcatNRCSemiComa(nrc);
                Console.WriteLine("Nrc concat seicoma" + nrcConcatWithSemiComa);
                PersonalDetail personalDetail = FindPersonalDetailByNrc(nrcConcatWithSemiComa);
                Console.WriteLine("personal Detail == null in db? " + (personalDetail == null));
                if (personalDetail == null)
                {
                    Console.WriteLine("Here personal detail null in db.............");
                    personalDetail = await GetPersonalInformationByNRC(nrc);
                    Console.WriteLine(" personal detail api call null............." + (personalDetail == null));

                }
                Console.WriteLine("personal detail api and nrc null" + (personalDetail == null));

                return personalDetail;
            }
            catch(Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding person by nrc. <<<<<<<<<<" + e);
                throw;
            }
        }

        public async Task<PersonalDetail> GetPersonalInformationByPhoneNumber(string phoneNumber)
        {
            _logger.LogInformation(">>>>>>>>>> [PersonDetailServiceImpl][GetPersonalInformationByPhoneNumber] Get personal information by phoneNumber. <<<<<<<<<<");
            try
            {
                //PersonalInformation personalInfo = await _personalDetailAPIService.GetPersonalInformationByNRC(nrc);
                PersonalDetail personalInfo = await _personalDetailAPIService.GetPersonalInformationByPhoneNumber(phoneNumber);
                return personalInfo;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding person by phone number. <<<<<<<<<<" + e);
                throw new HttpRequestException($"Failed to send message. Status code: {e.StatusCode}");
            }
        }

        public async Task<PersonalDetail> GetPersonalInformationByPhoneNumberInDBAndAPI(string phoneNumber)
        {
            try
            {
                _logger.LogInformation(">>>>>>>>>> [PersonDetailServiceImpl][GetPersonalInformationByPhoneNumberInDBAndAPI] Get personal information by phoneNumber in database and api. <<<<<<<<<<");
                string phoneNumberWithCountryCode = Utility.MakePhoneNumberWithCountryCode(phoneNumber);
                PersonalDetail personalDetail = FindPersonalDetailByPhoneNumber(phoneNumberWithCountryCode);
                Console.WriteLine("Here phone numer null in db: " + (personalDetail == null));
                if (personalDetail == null)
                {
                    Console.WriteLine("Here personal phone nulll");
                    personalDetail = await GetPersonalInformationByPhoneNumber(HttpUtility.UrlEncode(phoneNumberWithCountryCode));
                    Console.WriteLine("api call phone null? " + (personalDetail == null));
                }
                Console.WriteLine("personal detail api and phone null" + (personalDetail == null));
                return personalDetail;
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding person by phonenumber in database and api. <<<<<<<<<<" + e);
                throw;
            }
        }

        public bool UpdatePhoneNumberByNrc(string nrc, string oldPhoneNumber, string newPhoneNumber)
        {
            try
            {
                _logger.LogInformation(">>>>>>>>>> [PersonDetailServiceImpl][UpdatePhoneNumberByNrc]Update phoneNumber by nrc in database. <<<<<<<<<<");
                Console.WriteLine("updated ph no nrc: " + nrc);
                PersonalDetail personalDetail = FindPersonalDetailByNrc(nrc);
                if (personalDetail == null)
                    return false;
                if (personalDetail.PhoneNumber != oldPhoneNumber)
                    return false;
                personalDetail.PhoneNumber = newPhoneNumber;
                Update(personalDetail);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding person by phonenumber in database and api. <<<<<<<<<<" + e);
                throw;
            }
        }
        
        public async Task<bool> ResetPhoneNumber(ResetPhonenumber resetPhonenumber)
        {
            try
            {
                _logger.LogInformation(">>>>>>>>>> [PersonDetailServiceImpl][ResetPhoneNumber] Reset phonenumber. <<<<<<<<<<");
                string nrc = Utility.MakeNRC(resetPhonenumber.NRCTownshipNumber+"/", resetPhonenumber.NRCTownshipInitial, resetPhonenumber.NRCType, resetPhonenumber.NRCNumber);
                if (await GetPersonalInformationByPhoneNumberInDBAndAPI(resetPhonenumber.NewPhonenumber) != null)
                {
                    Console.WriteLine("here new phone not null........");
                    return false;
                }

                PersonalDetail personalDetail = await GetPersonalInformationByNRCInDBAndAPI(nrc);
                if (personalDetail == null)
                {
                    Console.WriteLine("here nrc  null........");

                    return false;
                }
                
                TaxValidation taxValidation = _taxValidationService.FindTaxValidationByNrc(nrc);
                if (taxValidation == null)
                {
                    Console.WriteLine("here taxvalidation  null........");

                    return false;
                }

                if (personalDetail.PersonalPkid != 0)
                {
                    return UpdatePhoneNumberByNrc(Utility.ConcatNRCSemiComa(nrc), Utility.MakePhoneNumberWithCountryCode(resetPhonenumber.OldPhonenumber), Utility.MakePhoneNumberWithCountryCode(resetPhonenumber.NewPhonenumber));
                }
                bool result = await _personalDetailAPIService.ResetPhoneNumber(nrc, HttpUtility.UrlEncode(Utility.MakePhoneNumberWithCountryCode(resetPhonenumber.OldPhonenumber)), HttpUtility.UrlEncode(Utility.MakePhoneNumberWithCountryCode(resetPhonenumber.NewPhonenumber)));
                return result;                
            }
            catch(Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when reset phonenumber. <<<<<<<<<<" + e);
                throw;
            }
        }

        public List<string> GetNRCTownshipInitials(string nrcTownshipNumber)
        {
            try
            {
                List<NRC_And_Township> NRC_And_Townships = _context.NRC_And_Townships.Where(Township => Township.NrcInitialCodeMyanmar == nrcTownshipNumber && Township.IsDeleted == false).ToList();
                Console.WriteLine("NRC_And_Townships length............." + NRC_And_Townships.Count);
                List<string> nrcTownshipInitials = new List<string>();
                foreach(NRC_And_Township nrcTownshipInitial in NRC_And_Townships)
                {
                    Console.WriteLine("NrcTownshipCodeMyn............." + nrcTownshipInitial.NrcTownshipCodeMyn);
                    nrcTownshipInitials.Add(nrcTownshipInitial.NrcTownshipCodeMyn);
                }
                return nrcTownshipInitials;

/*List<string> nrcTownshipInitials = await _personalDetailAPIService.GetNrcTownshipInitials(nrcTownshipNumber);
return nrcTownshipInitials;*/
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException($"Failed to send message. Status code: {e.StatusCode}");
            }
        }
    }
}
