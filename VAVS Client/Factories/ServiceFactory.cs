using VAVS_Client.Services;

namespace VAVS_Client.Factories
{
    public interface ServiceFactory
    {
        UserService CreateUserService();
        LoginAuthService CreateLoginAuthService();
        VehicleStandardValueService CreateVehicleStandardValueService();
        DeviceInfoService CreateDeviceInfoService();
        TaxPayerInfoService CreateTaxPayerInfoService();
        DeviceInfoDBService CreateDeviceInfoDBService();
        LoginAuthDBService CreateLoginAuthDBService();
        LoginUserInfoDBService CreateLoginUserInfoDBService();

        FileService CreateFileService();
        PersonalDetailService CreatePersonalDetailService();
        StateDivisionService CreateStateDivisionService();
        TownshipService CreateTownshipService();
        SMSVerificationService CreateSMSVerificationService();
        TaxCalculationService CreateTaxCalculationService();
        TaxValidationService CreateTaxValidationService();
        ResetPhoneNumberAuthService CreateResetPhoneNumberAuthService();
        SessionService CreateSessionServiceService();
        FinancialYearService CreateFinancialYearService();
        NRCANDTownshipService CreateNRCANDTownshipService();

    }
}
