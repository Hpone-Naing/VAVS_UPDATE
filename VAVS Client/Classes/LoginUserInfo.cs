namespace VAVS_Client.Classes
{
    public class LoginUserInfo
    {
        public TaxVehicleInfo? TaxVehicleInfo { get; set; }
        public DateTime? LoggedInTime { get; set; }
        public bool? RememberMe { get; set; }
        
        public bool IsTaxVehicleInfoNull()
        {
            return this == null || TaxVehicleInfo == null;
        }
    }
}
