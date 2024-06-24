using VAVS_Client.Data;

namespace VAVS_Client.Services.Impl
{
    public class FuelTypeServiceImpl : AbstractServiceImpl<Fuel>, FuelTypeService
    {

        private readonly ILogger<FuelTypeServiceImpl> _logger;

        public FuelTypeServiceImpl(VAVSClientDBContext context, ILogger<FuelTypeServiceImpl> logger) : base(context, logger)
        {
            _logger = logger;
        }
        public Fuel FindFuelByFuelType(string fuelType)
        {
            _logger.LogInformation(">>>>>>>>>> [VehicleStandardValueServiceImpl][FindFuelByFuelType] Find Fuel by FuelType. <<<<<<<<<<");
            try
            {
                _logger.LogInformation(">>>>>>>>>> Success. Find Fuel by FuelType. <<<<<<<<<<");
                return FindByString("FuelType", fuelType);
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding Fuel by FuelType. <<<<<<<<<<" + e);
                throw;
            }
        }
    }
}
