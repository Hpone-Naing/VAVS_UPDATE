using VAVS_Client.APIService;

namespace VAVS_Client.APIFactory
{
    public interface APIServiceFactory
    {
        PersonalDetailAPIService CreatePersonalDetailAPIService();
        VehicleStandardValueAPIService CreateVehicleStandardValueAPIService();

    }
}
