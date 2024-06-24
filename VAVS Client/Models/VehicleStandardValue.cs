 using System.ComponentModel;

namespace VAVS_Client.Models
{
    [Table("TB_VehicleStandardValue")]
    public class VehicleStandardValue
    {
        [Key]
        public int VehicleStandardValuePkid { get; set; }

        [StringLength(200)]
        public string? Manufacturer { get; set; }

        [StringLength(50)]
        public string? CountryOfMade { get; set; }

        [StringLength(200)]
        public string? VehicleBrand { get; set; }

        [StringLength(200)]
        public string? BuildType { get; set; }

        [StringLength(4)]
        public string? ModelYear { get; set; }

        [StringLength(30)]
        public string? EnginePower { get; set; }

        [StringLength(10)]
        public string? StandardValue { get; set; }

        [StringLength(10)]
        public string? VehicleNumber { get; set; }

        [StringLength(50)]
        public string? OfficeLetterNo { get; set; }

        [StringLength(50)]
        public string? AttachFileName { get; set; }

        [StringLength(200)]
        public string? Remarks { get; set; }

        public DateTime? EntryDate { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(10)]
        public string? CreatedBy { get; set; }

        [ForeignKey("StateDivision")]
        [DisplayName("State Division")]
        public int StateDivisionPkid { get; set; }
        public virtual StateDivision StateDivision { get; set; }

        [ForeignKey("Fuel")]
        [DisplayName("Fuel Type")]
        public int FuelTypePkid { get; set; }
        public virtual Fuel Fuel { get; set; }

        [NotMapped]
        public IFormFile? NrcImageFile { get; set; }

        [NotMapped]
        public IFormFile? CensusImageFile { get; set; }

        [NotMapped]
        public IFormFile? TransactionContractImageFile { get; set; }

        [NotMapped]
        public IFormFile? OwnerBookImageFile { get; set; }

        [NotMapped]
        public IFormFile? WheelTagImageFile { get; set; }

        [NotMapped]
        public IFormFile? VehicleImageFile { get; set; }

        public bool IsImageFilesNotNull()
        {
            return CensusImageFile != null && TransactionContractImageFile != null && OwnerBookImageFile != null && WheelTagImageFile != null && VehicleImageFile != null;
        }
    }
}
