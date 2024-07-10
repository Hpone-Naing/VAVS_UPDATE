namespace VAVS_Client.APIService
{
    public interface VehicleStandardValueAPIService
    {
        Task<VehicleStandardValue> GetVehicleValueByVehicleNumber(string carNumber);
        Task<VehicleStandardValue> GetVehicleValue(string manufacturer, string buildType, string fuelType, string vehicleBrand, string modelYear, string enginePower);
        Task<List<string>> GetVehicleByMadeModel(string searchString);
        Task<List<string>> GetModelYears(string makeModel);
        Task<List<string>> GetModelYears(string makeModel, string brandName);
        Task<List<string>> GetBrandNames(string makeModel);
        Task<List<VehicleStandardValue>> GetVehicleStandardValueByModelAndYear(string makeModel, string makeyear);
        Task<List<VehicleStandardValue>> GetVehicleStandardValueByModelAndBrandAndYear(string makeModel, string brand, string makeyear);
        Task<string> GetVehicleChessisNumber(string carNumber);

    }
}
