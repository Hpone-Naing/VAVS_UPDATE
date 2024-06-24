using VAVS_Client.Paging;

namespace VAVS_Client.ViewModels
{
    public class SearchVehicleStandardValueViewModel
    {
        public string SearchString { get; set; }
        public string Manufacturer { get; set; }
        public string BuildType { get; set; }
        public string FuelType { get; set; }
        public string VehicleBrand { get; set; }
        public string ModelYear { get; set; }
        public string EnginePower { get; set; }
        public PagingList<VehicleStandardValue> VehicleStandardValues { get; set; }

    }
}
