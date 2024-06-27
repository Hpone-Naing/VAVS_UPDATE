using FireSharp.Interfaces;
using VAVS_Client.APIFactory;
using VAVS_Client.APIService;
using VAVS_Client.Data;


namespace VAVS_Client.Factories.Impl
{
    public class ServiceFactoryImpl : ServiceFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private IFirebaseConfig _firebaseConfig;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly HttpClient _httpClient;
        private readonly VAVSClientDBContext _context;

        private readonly FileService _fileService;
        private readonly APIServiceFactory _apiServiceFactory;
        private readonly SessionService _sessionService;
        private readonly FinancialYearService _financialYearService;

        public ServiceFactoryImpl(VAVSClientDBContext context, IFirebaseConfig firebaseConfig, HttpClient httpClient, ILoggerFactory loggerFactory, FileService fileService, IWebHostEnvironment hostEnvironment, APIServiceFactory apiServiceFactory, SessionService sessionService, FinancialYearService financialYearService)
        {
            _context = context;
            _firebaseConfig = firebaseConfig;
            _httpClient = httpClient;
            _loggerFactory = loggerFactory;
            _hostEnvironment = hostEnvironment;
            _fileService = fileService;
            _apiServiceFactory = apiServiceFactory;
            _sessionService = sessionService;
            _financialYearService = financialYearService;
        }

        public UserService CreateUserService()
        {
            ILogger<UserServiceImpl> userLogger = new Logger<UserServiceImpl>(_loggerFactory);
            return new UserServiceImpl(_context, userLogger);
        }

        public VehicleStandardValueService CreateVehicleStandardValueService()
        {
            ILogger<VehicleStandardValueServiceImpl> vehicleStandardValueLogger = new Logger<VehicleStandardValueServiceImpl>(_loggerFactory);
            return new VehicleStandardValueServiceImpl(_httpClient, _context, vehicleStandardValueLogger, _apiServiceFactory.CreateVehicleStandardValueAPIService());
        }

        public DeviceInfoService CreateDeviceInfoService()
        {
            return new DeviceInfoServiceImpl(_firebaseConfig);
        }

        public TaxPayerInfoService CreateTaxPayerInfoService()
        {
            return new TaxPayerInfoServiceImpl(_firebaseConfig);
        }

        public LoginAuthService CreateLoginAuthService()
        {
            return new LoginAuthServiceImpl(_firebaseConfig);
        }

        public DeviceInfoDBService CreateDeviceInfoDBService()
        {
            ILogger<DeviceInfoDBServiceImpl> deviceInfoLogger = new Logger<DeviceInfoDBServiceImpl>(_loggerFactory);
            return new DeviceInfoDBServiceImpl(_context, deviceInfoLogger);
        }

        public LoginAuthDBService CreateLoginAuthDBService()
        {
            ILogger<LoginAuthDBServiceImpl> loginAuthLogger = new Logger<LoginAuthDBServiceImpl>(_loggerFactory);
            return new LoginAuthDBServiceImpl(_context, loginAuthLogger);
        }

        public LoginUserInfoDBService CreateLoginUserInfoDBService()
        {
            ILogger<LoginUserInfoDBServiceImpl> loginUserInfoLogger = new Logger<LoginUserInfoDBServiceImpl>(_loggerFactory);
            return new LoginUserInfoDBServiceImpl(_context, loginUserInfoLogger);
        }
        public FileService CreateFileService()
        {
            return new FileServiceImpl(_hostEnvironment);
        }

        public PersonalDetailService CreatePersonalDetailService()
        {
            ILogger<PersonalDetailServiceImpl> personalDetailLogger = new Logger<PersonalDetailServiceImpl>(_loggerFactory);
            return new PersonalDetailServiceImpl(_context, _httpClient, _fileService, personalDetailLogger, _apiServiceFactory.CreatePersonalDetailAPIService(), CreateTaxValidationService());
        }
        public StateDivisionService CreateStateDivisionService()
        {
            ILogger<StateDivisionServiceImpl> stateDivisionLogger = new Logger<StateDivisionServiceImpl>(_loggerFactory);
            return new StateDivisionServiceImpl(_context, stateDivisionLogger);
        }
        public TownshipService CreateTownshipService()
        {
            ILogger<TownshipServiceImpl> townshipServiceLogger = new Logger<TownshipServiceImpl>(_loggerFactory);
            return new TownshipServiceImpl(_context, townshipServiceLogger);
        }

        public SMSVerificationService CreateSMSVerificationService()
        {
            return new SMSVerificationServiceImpl(_httpClient);
        }

        public FuelTypeService CreateFuelTypeService()
        {
            ILogger<FuelTypeServiceImpl> fuelServiceLogger = new Logger<FuelTypeServiceImpl>(_loggerFactory);
            return new FuelTypeServiceImpl(_context, fuelServiceLogger);
        }
        public TaxCalculationService CreateTaxCalculationService()
        {
            ILogger<TaxCalculationServiceImpl> taxCalculationServiceLogger = new Logger<TaxCalculationServiceImpl>(_loggerFactory);
            return new TaxCalculationServiceImpl(_context, _httpClient, taxCalculationServiceLogger, CreatePersonalDetailService(), CreateTownshipService(), CreateStateDivisionService(), CreateVehicleStandardValueService(), CreateFuelTypeService(), CreateLoginUserInfoDBService(), _financialYearService);
        }
        public TaxValidationService CreateTaxValidationService()
        {
            ILogger<TaxValidationServiceImpl> taxValidationLogger = new Logger<TaxValidationServiceImpl>(_loggerFactory);
            return new TaxValidationServiceImpl(_context, taxValidationLogger, CreateTaxPayerInfoService(), _sessionService);
        }

        public ResetPhoneNumberAuthService CreateResetPhoneNumberAuthService()
        {
            return new ResetPhoneNumberAuthServiceImpl(_firebaseConfig);
        }

        public SessionService CreateSessionServiceService()
        {
            return _sessionService;
        }

        public FinancialYearService CreateFinancialYearService()
        {
            return _financialYearService;
        }

        public NRCANDTownshipService CreateNRCANDTownshipService()
        {
            ILogger<NRCANDTownshipServiceImpl> nrcAndTownshipLogger = new Logger<NRCANDTownshipServiceImpl>(_loggerFactory);
            return new NRCANDTownshipServiceImpl(_context, nrcAndTownshipLogger);
        }

        public TaxPersonImageService CreateTaxPersonImageService()
        {
            ILogger<TaxPersonImageServiceImpl> TaxPersonImageServiceImplLogger = new Logger<TaxPersonImageServiceImpl>(_loggerFactory);
            return new TaxPersonImageServiceImpl(_context, TaxPersonImageServiceImplLogger);
        }
    }
}
