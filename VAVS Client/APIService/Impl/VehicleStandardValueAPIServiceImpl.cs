using System.Net;
using System.Text.Json;
using VAVS_Client.Util;
using Newtonsoft.Json.Linq;

namespace VAVS_Client.APIService.Impl
{
    public class VehicleStandardValueAPIServiceImpl : VehicleStandardValueAPIService
    {
        private readonly ILogger<VehicleStandardValueAPIServiceImpl> _logger;
        private readonly HttpClient _httpClient;

        public VehicleStandardValueAPIServiceImpl(HttpClient httpClient, ILogger<VehicleStandardValueAPIServiceImpl> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<VehicleStandardValue> GetVehicleValueByVehicleNumber(string carNumber)
        {
            string apiKey = Utility.SEARCH_VEHICLE_STANDARD_VALUE_API_KEY;
            string baseUrl = "http://203.81.89.218:99/VehicleStandardAPI/api/VehicleStandard/GetVehicleByCarNumber";
            string url = $"{baseUrl}?carNumber={carNumber}&apiKey={apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            Console.WriteLine("success state code: " + response.StatusCode);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                JsonDocument doc = JsonDocument.Parse(json);
                JsonElement root = doc.RootElement;
                return new VehicleStandardValue()
                {
                    Manufacturer = root.GetProperty("carManufacturer").GetString(),
                    VehicleBrand = root.GetProperty("carBrand").GetString(),
                    Grade = root.GetProperty("grade").GetString(),
                    ModelYear = root.GetProperty("modelYear").GetString(),
                    StandardValue =  root.GetProperty("standardValue").GetString(),
                    BuildType = root.GetProperty("builtType").GetString(),
                    EnginePower = root.GetProperty("enginePower").GetString(),
                    //VehicleNumber = root.GetProperty("carNumber").GetString(),
                    Fuel = new Fuel()
                    {
                        FuelType = root.GetProperty("fuelType").GetString()
                    }
                };
            }

            Console.WriteLine("fail state code: " + response.StatusCode);
            Console.WriteLine($"Failed to send message. Status code: {response.StatusCode}");
            throw new HttpRequestException($"Failed to send message. Status code: {response.StatusCode}");

        }

        public async Task<List<string>> GetVehicleByMadeModel(string searchString)
        {
            Console.WriteLine("searchString api........" + searchString);
            string apiKey = Utility.SEARCH_VEHICLE_STANDARD_VALUE_API_KEY;
            string baseUrl = "http://203.81.89.218:99/VehicleStandardAPI/api/VehicleStandard/GetVehicleMadeModel";
            string url = $"{baseUrl}?mademodel={searchString}&apiKey={apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            Console.WriteLine("success state code: " + response.StatusCode);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<string> vehicleModels = JsonSerializer.Deserialize<List<string>>(json);
                return vehicleModels;
            }

            Console.WriteLine("fail state code: " + response.StatusCode);
            Console.WriteLine($"Failed to send message. Status code: {response.StatusCode}");
            throw new HttpRequestException($"Failed to send message. Status code: {response.StatusCode}");
        }

        public async Task<VehicleStandardValue> GetVehicleValue(string manufacturer, string buildType, string fuelType, string vehicleBrand, string modelYear, string enginePower)
        {
            string apiKey = Utility.SEARCH_VEHICLE_STANDARD_VALUE_API_KEY;
            string baseUrl = "http://203.81.89.218:99/VehicleStandardAPI/api/VehicleStandard/GetVehicleStandardValue";
            string url = $"{baseUrl}?manufacturer={manufacturer}&buildType={buildType}&fuelType={fuelType}&vehicleBrand={vehicleBrand}&modelYear={modelYear}&enginePower={enginePower}&apiKey={apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            Console.WriteLine("success state code: " + response.StatusCode);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();
                JObject jsonObject = JObject.Parse(responseJson);
                string standardValue = (string)jsonObject["standardValue"];
                return new VehicleStandardValue()
                {
                    Manufacturer = manufacturer,
                    BuildType = buildType,
                    VehicleBrand = vehicleBrand,
                    ModelYear = modelYear,
                    EnginePower = enginePower,
                    StandardValue = standardValue,
                    Fuel = new Fuel()
                    {
                        FuelType = fuelType
                    }
                };
            }

            Console.WriteLine("fail state code: " + response.StatusCode);
            Console.WriteLine($"Failed to send message. Status code: {response.StatusCode}");
            throw new HttpRequestException($"Failed to send message. Status code: {response.StatusCode}");

        }

        public async Task<List<string>> GetModelYears(string makeModel)
        {
            Console.WriteLine("searchString api........" + makeModel);
            string apiKey = Utility.SEARCH_VEHICLE_STANDARD_VALUE_API_KEY;
            string baseUrl = "http://203.81.89.218:99/VehicleStandardAPI/api/VehicleStandard/GetVehicleMadeYearbyMadeModel";
            string url = $"{baseUrl}?mademodel={makeModel}&apiKey={apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            Console.WriteLine("success state code: " + response.StatusCode);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<string> vehicleModels = JsonSerializer.Deserialize<List<string>>(json);
                return vehicleModels;
            }

            Console.WriteLine("fail state code: " + response.StatusCode);
            Console.WriteLine($"Failed to send message. Status code: {response.StatusCode}");
            throw new HttpRequestException($"Failed to send message. Status code: {response.StatusCode}");
        }

        public async Task<List<string>> GetModelYears(string makeModel, string brandName)
        {
            Console.WriteLine("searchString api........" + makeModel);
            string apiKey = Utility.SEARCH_VEHICLE_STANDARD_VALUE_API_KEY;
            string baseUrl = "http://203.81.89.218:99/VehicleStandardAPI/api/VehicleStandard/GetModelYearListByManufacturerAndCarBrand";
            string url = $"{baseUrl}?manufacturer={makeModel}&brand={brandName}&apiKey={apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            Console.WriteLine("success state code: " + response.StatusCode);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<string> vehicleModels = JsonSerializer.Deserialize<List<string>>(json);
                return vehicleModels;
            }

            Console.WriteLine("fail state code: " + response.StatusCode);
            Console.WriteLine($"Failed to send message. Status code: {response.StatusCode}");
            throw new HttpRequestException($"Failed to send message. Status code: {response.StatusCode}");
        }

        public async Task<List<string>> GetBrandNames(string makeModel)
        {
            Console.WriteLine("searchString api........" + makeModel);
            string apiKey = Utility.SEARCH_VEHICLE_STANDARD_VALUE_API_KEY;
            string baseUrl = "http://203.81.89.218:99/VehicleStandardAPI/api/VehicleStandard/GetCarBrandListByManufacturer";
            string url = $"{baseUrl}?manufacturer={makeModel}&apiKey={apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            Console.WriteLine("success state code: " + response.StatusCode);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<string> vehicleModels = JsonSerializer.Deserialize<List<string>>(json);
                return vehicleModels;
            }

            Console.WriteLine("fail state code: " + response.StatusCode);
            Console.WriteLine($"Failed to send message. Status code: {response.StatusCode}");
            throw new HttpRequestException($"Failed to send message. Status code: {response.StatusCode}");
        }

        public async Task<List<VehicleStandardValue>> GetVehicleStandardValueByModelAndYear(string makeModel, string makeyear)
        {
            Console.WriteLine("searchString api........" + makeModel);
            Console.WriteLine("searchString api........" + makeyear);
            string apiKey = Utility.SEARCH_VEHICLE_STANDARD_VALUE_API_KEY;
            string baseUrl = "http://203.81.89.218:99/VehicleStandardAPI/api/VehicleStandard/GetVehicleByMadeModelandMadeYear";
            string url = $"{baseUrl}?mademodel={makeModel}&madeYear={makeyear}&apiKey={apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            Console.WriteLine("success state code: " + response.StatusCode);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("here success code...................");
                string json = await response.Content.ReadAsStringAsync();
                var vehicleModels = new List<VehicleStandardValue>();

                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    JsonElement root = doc.RootElement;
                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        Console.WriteLine("valuekind == array..................." + (root.ValueKind == JsonValueKind.Array));

                        foreach (JsonElement element in root.EnumerateArray())
                        {
                            var vehicle = new VehicleStandardValue
                            {
                                VehicleStandardValuePkid = element.GetProperty("carMasterPkid").GetInt32(),
                                Manufacturer = element.GetProperty("carManufacturer").GetString(),
                                VehicleBrand = element.GetProperty("carBrand").GetString(),
                                Grade = element.GetProperty("grade").GetString(),
                                ModelYear = element.GetProperty("modelYear").GetString(),
                                StandardValue = element.GetProperty("standardValue").GetString(),
                                BuildType = element.GetProperty("builtType").GetString(),
                                EnginePower = element.GetProperty("enginePower").GetString(),
                                Fuel =  new Fuel()
                                {
                                   FuelType = element.GetProperty("fuelType").GetString()
                                }
                            };

                            vehicleModels.Add(vehicle);
                        }
                    }
                }

                return vehicleModels;

            }

            Console.WriteLine("fail state code: " + response.StatusCode);
            Console.WriteLine($"Failed to send message. Status code: {response.StatusCode}");
            throw new HttpRequestException($"Failed to send message. Status code: {response.StatusCode}");
        }

        public async Task<List<VehicleStandardValue>> GetVehicleStandardValueByModelAndBrandAndYear(string makeModel, string brand, string makeyear)
        {
            Console.WriteLine("searchString api........" + makeModel);
            Console.WriteLine("searchString api........" + makeyear);
            Console.WriteLine("searchString api........" + brand);
            string apiKey = Utility.SEARCH_VEHICLE_STANDARD_VALUE_API_KEY;
            string baseUrl = "http://203.81.89.218:99/VehicleStandardAPI/api/VehicleStandard/GetVehicleListByCarDetails";
            string url = $"{baseUrl}?manufacturer={makeModel}&brand={brand}&modelyear={makeyear}&apiKey={apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            Console.WriteLine("success state code: " + response.StatusCode);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("here success code...................");
                string json = await response.Content.ReadAsStringAsync();
                var vehicleModels = new List<VehicleStandardValue>();

                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    JsonElement root = doc.RootElement;
                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        Console.WriteLine("valuekind == array..................." + (root.ValueKind == JsonValueKind.Array));

                        foreach (JsonElement element in root.EnumerateArray())
                        {
                            var vehicle = new VehicleStandardValue
                            {
                                VehicleStandardValuePkid = element.GetProperty("carMasterPkid").GetInt32(),
                                Manufacturer = element.GetProperty("carManufacturer").GetString(),
                                VehicleBrand = element.GetProperty("carBrand").GetString(),
                                Grade = element.GetProperty("grade").GetString(),
                                ModelYear = element.GetProperty("modelYear").GetString(),
                                StandardValue = element.GetProperty("standardValue").GetString(),
                                BuildType = element.GetProperty("builtType").GetString(),
                                EnginePower = element.GetProperty("enginePower").GetString(),
                                Fuel = new Fuel()
                                {
                                    FuelType = element.GetProperty("fuelType").GetString()
                                }
                            };

                            vehicleModels.Add(vehicle);
                        }
                    }
                }

                return vehicleModels;

            }

            Console.WriteLine("fail state code: " + response.StatusCode);
            Console.WriteLine($"Failed to send message. Status code: {response.StatusCode}");
            throw new HttpRequestException($"Failed to send message. Status code: {response.StatusCode}");
        }
    }
}
