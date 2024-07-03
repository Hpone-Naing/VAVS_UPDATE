using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using VAVS_Client.APIService;
using VAVS_Client.Data;
using VAVS_Client.Paging;
using VAVS_Client.Util;

namespace VAVS_Client.Services.Impl
{
    public class VehicleStandardValueServiceImpl : AbstractServiceImpl<VehicleStandardValue>, VehicleStandardValueService
    {
        private readonly ILogger<VehicleStandardValueServiceImpl> _logger;
        private readonly HttpClient _httpClient;
        private readonly VehicleStandardValueAPIService _vehicleStandardValueAPIService;

        public VehicleStandardValueServiceImpl(HttpClient httpClient, VAVSClientDBContext context, ILogger<VehicleStandardValueServiceImpl> logger, VehicleStandardValueAPIService vehicleStandardValueAPIService) : base(context, logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _vehicleStandardValueAPIService = vehicleStandardValueAPIService;
        }

        public List<VehicleStandardValue> GetAllVehicles()
        {
            _logger.LogInformation(">>>>>>>>>> [VehicleStandardValueServiceImpl][GetAllVehicles] Get VehicleStandardValue list. <<<<<<<<<<");
            try
            {
                _logger.LogInformation($">>>>>>>>>> Success. Get VehicleStandardValue list. <<<<<<<<<<");
                return GetAll().Where(vehicle => vehicle.IsDeleted == false).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when retrieving VehicleStandardValue list. <<<<<<<<<<" + e);
                throw;
            }
        }

        public List<VehicleStandardValue> GetAllVehiclesEgerLoad()
        {
            _logger.LogInformation(">>>>>>>>>> [VehicleStandardValueServiceImpl][GetAllVehiclesEgerLoad] Get VehicleStandardValue eger load list. <<<<<<<<<<");
            try
            {
                _logger.LogInformation($">>>>>>>>>> Success. Get VehicleStandardValue eger load list. <<<<<<<<<<");
                return _context.VehicleStandardValues.Where(vehicle => vehicle.IsDeleted == false).Include(fuel => fuel.Fuel).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when retrieving VehicleStandardValue eger load list. <<<<<<<<<<" + e);
                throw;
            }
        }
        public PagingList<VehicleStandardValue> GetAllVehiclesWithPagin(string searchString, int? pageNo, int PageSize)
        {
            _logger.LogInformation(">>>>>>>>>> [VehicleStandardValueServiceImpl][GetAllVehiclesWithPagin] SearchAll or GetAll VehicleStandardValue paginate eger load list. <<<<<<<<<<");
            try
            {
                List<VehicleStandardValue> vehicleDatas = GetAllVehiclesEgerLoad();
            List<VehicleStandardValue> resultList = new List<VehicleStandardValue>();
            if (searchString != null && !String.IsNullOrEmpty(searchString))
            {
                    _logger.LogInformation($">>>>>>>>>> Get searchAll result VehicleStandardValue paginate eger load list. <<<<<<<<<<");
                    try
                    {
                        _logger.LogInformation($">>>>>>>>>> Success. Get searchAll result VehicleStandardValue paginate eger load list. <<<<<<<<<<");
                        resultList = vehicleDatas.Where(vehicle => IsSearchDataContained(vehicle, searchString))
                            .AsQueryable()
                            .ToList();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(">>>>>>>>>> Error occur. Get searchAll result VehicleStandardValue paginate eger load list. <<<<<<<<<<" + e);
                        throw;
                    }
                }
            else
            {
                    _logger.LogInformation($">>>>>>>>>> GetAll VehicleStandardValue paginate eger load list. <<<<<<<<<<");
                    try
                    {
                        _logger.LogInformation($">>>>>>>>>> Success. GetAll VehicleStandardValue paginate eger load list. <<<<<<<<<<");
                        resultList = vehicleDatas.AsQueryable().ToList();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(">>>>>>>>>> Error occur. GetAll VehicleStandardValue paginate eger load list. <<<<<<<<<<" + e);
                        throw;
                    }
                }
                _logger.LogInformation($">>>>>>>>>> Success. SearchAll or GetAll SpecialEventInvestigationDept paginate eger load list. <<<<<<<<<<");
                return GetAllWithPagin(resultList, pageNo, PageSize);
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur. SearchAll or GetAll VehicleStandardValue paginate eger load list. <<<<<<<<<<" + e);
                throw;
            }
        }

        public VehicleStandardValue FindVehicleStandardValueById(int id)
        {
            _logger.LogInformation(">>>>>>>>>> [VehicleStandardValueServiceImpl][FindVehicleStandardValueById] Find VehicleStandardValue by pkId. <<<<<<<<<<");
            try
            {
                _logger.LogInformation(">>>>>>>>>> Success. Find VehicleStandardValue by pkId. <<<<<<<<<<");
                return FindById(id);
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding VehicleStandardValue by pkId. <<<<<<<<<<" + e);
                throw;
            }
        }

        public VehicleStandardValue FindVehicleStandardValueByIdEgerLoad(int id)
        {
            _logger.LogInformation(">>>>>>>>>> [VehicleStandardValueServiceImpl][FindVehicleStandardValueByIdEgerLoad] Find VehicleStandardValue by pkId with eger load. <<<<<<<<<<");
            try
            {
                _logger.LogInformation(">>>>>>>>>> Success. Find VehicleStandardValue by pkId with eger load. <<<<<<<<<<");
                return _context.VehicleStandardValues.Where(VehicleStandardValue => VehicleStandardValue.IsDeleted == false)
                           .FirstOrDefault(vehicle => vehicle.VehicleStandardValuePkid == id);
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding VehicleStandardValue by pkId with eger load. <<<<<<<<<<" + e);
                throw;
            }
        }

        public VehicleStandardValue FindVehicleStandardValueByIdContainSoftDeleteEgerLoad(int id)
        {
            _logger.LogInformation(">>>>>>>>>> [VehicleStandardValueServiceImpl][FindVehicleStandardValueByIdContainSoftDeleteEgerLoad] Find VehicleStandardValue by pkId with eger load. <<<<<<<<<<");
            try
            {
                _logger.LogInformation(">>>>>>>>>> Success. Find VehicleStandardValue by pkId with eger load. <<<<<<<<<<");
                return _context.VehicleStandardValues.Where(VehicleStandardValue => VehicleStandardValue.IsDeleted == false)
                           .FirstOrDefault(vehicle => vehicle.VehicleStandardValuePkid == id);
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding VehicleStandardValue by pkId with eger load. <<<<<<<<<<" + e);
                throw;
            }
        }


        public VehicleStandardValue FindVehicleStandardValueByIdYBSTableEgerLoad(int id)
        {
            _logger.LogInformation(">>>>>>>>>> [VehicleStandardValueServiceImpl][FindVehicleStandardValueByIdYBSTableEgerLoad] Find VehicleStandardValue by pkId with YBSTable eger load. <<<<<<<<<<");
            try
            {
                _logger.LogInformation(">>>>>>>>>> Success. Find VehicleStandardValue by pkId with YBSTable eger load. <<<<<<<<<<");
                return _context.VehicleStandardValues.Where(VehicleStandardValue => VehicleStandardValue.IsDeleted == false)
                           .FirstOrDefault(vehicle => vehicle.VehicleStandardValuePkid == id);
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding VehicleStandardValue by pkId with YBSTable eger load. <<<<<<<<<<" + e);
                throw;
            }
        }        

        public VehicleStandardValue FindVehicleByVehicleNumber(string vehicleNumer)
        {
            _logger.LogInformation(">>>>>>>>>> [VehicleStandardValueServiceImpl][FindVehicleByVehicleNumber] Find VehicleStandardValue by vehicleNumber. <<<<<<<<<<");
            try
            {
                _logger.LogInformation(">>>>>>>>>> Success. Find VehicleStandardValue by vehicleNumber. <<<<<<<<<<");
                return FindByString("VehicleNumber", vehicleNumer);
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding VehicleStandardValue by vehicleNumber. <<<<<<<<<<" + e);
                throw;
            }
        }

        public VehicleStandardValue FindVehicleByVehicleNumberEgerLoad(string vehicleNumer)
        {
            _logger.LogInformation(">>>>>>>>>> [VehicleStandardValueServiceImpl][FindVehicleByVehicleNumber] Find VehicleStandardValue by vehicleNumber. <<<<<<<<<<");
            try
            {
                _logger.LogInformation(">>>>>>>>>> Success. Find VehicleStandardValue by vehicleNumber. <<<<<<<<<<");
                return _context.VehicleStandardValues.Where(vehicle => vehicle.VehicleNumber == vehicleNumer).Include(fuel => fuel.Fuel).FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding VehicleStandardValue by vehicleNumber. <<<<<<<<<<" + e);
                throw;
            }
        }

        public async Task<VehicleStandardValue> GetVehicleValueByVehicleNumber(string carNumber)
        {
            try
            {
                VehicleStandardValue vehicleStandardValue = await _vehicleStandardValueAPIService.GetVehicleValueByVehicleNumber(carNumber);
                return vehicleStandardValue;
            }
            catch(HttpRequestException e)
            {
                throw new HttpRequestException($"Failed to send message. Status code: {e.StatusCode}");
            }
        }

        public async Task<VehicleStandardValue> GetVehicleValueByVehicleNumberInDBAndAPI(string carNumber)
        {
            try
            {
                VehicleStandardValue vehicleStandardValue = FindVehicleByVehicleNumberEgerLoad(carNumber);
                if(vehicleStandardValue == null)
                {
                    vehicleStandardValue = await _vehicleStandardValueAPIService.GetVehicleValueByVehicleNumber(carNumber);
                }
                if(vehicleStandardValue != null)
                {
                    vehicleStandardValue.VehicleNumber = carNumber;
                }
                return vehicleStandardValue;
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException($"Failed to send message. Status code: {e.StatusCode}");
            }
        }

        public async Task<VehicleStandardValue> GetVehicleValue(string manufacturer, string buildType, string fuelType, string vehicleBrand, string modelYear, string enginePower)
        {
            try
            {
                VehicleStandardValue vehicleStandardValue = await _vehicleStandardValueAPIService.GetVehicleValue(manufacturer, buildType, fuelType, vehicleBrand, modelYear, enginePower);
                return vehicleStandardValue;
            }
            catch(HttpRequestException e)
            {
                throw new HttpRequestException($"Failed to send message. Status code: {e.StatusCode}");
            }
        }

        public bool CreateVehicleStandardValue(VehicleStandardValue vehicleStandardValue)
        {
            _logger.LogInformation(">>>>>>>>>> [VehicleStandardValueServiceImpl][CreateVehicleStandardValue] Create VehicleStandardValue. <<<<<<<<<<");
            try
            {

                vehicleStandardValue.IsDeleted = false;
                vehicleStandardValue.CreatedBy = "admin";
                vehicleStandardValue.CreatedDate = DateTime.Now;
                _logger.LogInformation($">>>>>>>>>> Success. Create VehicleStandardValue. <<<<<<<<<<");
                return Create(vehicleStandardValue);
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when creating VehicleStandardValue. <<<<<<<<<<" + e);
                throw;
            }
        }

        public async Task<List<string>> GetMadeModel(string searchString)
        {
            try
            {
                List<string> madeModels = await _vehicleStandardValueAPIService.GetVehicleByMadeModel(searchString);
                return madeModels;
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException($"Failed to send message. Status code: {e.StatusCode}");
            }
        }

        public async Task<List<string>> GetModelYear(string madeModel)
        {
            try
            {
                List<string> years = await _vehicleStandardValueAPIService.GetModelYears(madeModel);
                return years;
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException($"Failed to send message. Status code: {e.StatusCode}");
            }
        }

        public async Task<List<VehicleStandardValue>>  GetVehicleStandardValueByModelAndYear(string madeModel, string modelYear)
        {
            try
            {
                List<VehicleStandardValue> vehicleStandardValues = await _vehicleStandardValueAPIService.GetVehicleStandardValueByModelAndYear(madeModel, modelYear);
                return vehicleStandardValues;
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException($"Failed to send message. Status code: {e.StatusCode}");
            }
        }

    }
}
