using VAVS_Client.Classes;

namespace VAVS_Client.Models
{
    [Table("TB_LoginUserInfo")]
    public class LoginUserInfo
    {
        [Key]
        public int LoginUserInfoId { get; set; }

        [StringLength(200)]
        public string? Token { get; set; }

        [StringLength(200)]
        public string? VehicleNumber { get; set; }

        [StringLength(200)]
        public string? Manufacturer { get; set; }

        [StringLength(200)]
        public string? VehicleBrand { get; set; }

        [StringLength(200)]
        public string? BuildType { get; set; }

        [StringLength(200)]
        public string? CountryOfMade { get; set; }

        [StringLength(200)]
        public string? ModelYear { get; set; }

        [StringLength(200)]
        public string? FuelType { get; set; }

        [StringLength(200)]
        public string? EnginePower { get; set; }

        [StringLength(200)]
        public string? StandardValue { get; set; }

        [StringLength(200)]
        public string? ContractValue { get; set; }

        [StringLength(200)]
        public string? TaxAmount { get; set; }

        [StringLength(200)]
        public string? Grade { get; set; }

        public DateTime? LoggedInTime { get; set; }
        public bool? RememberMe { get; set; }

       
    }
}
