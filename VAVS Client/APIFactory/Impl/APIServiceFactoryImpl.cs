using VAVS_Client.APIService;
using VAVS_Client.APIService.Impl;

namespace VAVS_Client.APIFactory.Impl
{
    public class APIServiceFactoryImpl : APIServiceFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly HttpClient _httpClient;

        public APIServiceFactoryImpl(HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            _httpClient = httpClient;
            _loggerFactory = loggerFactory;
        }

        public PersonalDetailAPIService CreatePersonalDetailAPIService()
        {
            ILogger<PersonalDetailAPIServiceImpl> personalDetailAPIServiceLogger = new Logger<PersonalDetailAPIServiceImpl>(_loggerFactory);
            return new PersonalDetailAPIServiceImpl(_httpClient, personalDetailAPIServiceLogger);
        }

        public VehicleStandardValueAPIService CreateVehicleStandardValueAPIService()
        {
            ILogger<VehicleStandardValueAPIServiceImpl> vehicleStandardValueAPIServiceLogger = new Logger<VehicleStandardValueAPIServiceImpl>(_loggerFactory);
            return new VehicleStandardValueAPIServiceImpl(_httpClient, vehicleStandardValueAPIServiceLogger);
        }
    }
}
