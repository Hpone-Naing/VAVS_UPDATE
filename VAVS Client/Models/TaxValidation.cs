using System.ComponentModel;

namespace VAVS_Client.Models
{
    [Table("TB_TaxValidation")]
    public class TaxValidation
    {
        [Key]
        public int TaxValidationPkid { get; set; }

        [StringLength(50)]
        public string? PersonTINNumber { get; set; }

        [StringLength(50)]
        public string? PersonNRC { get; set; }


        [StringLength(10)]
        public string? VehicleNumber { get; set; }

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

        [StringLength(50)]
        public string? FuelType { get; set; }

        [StringLength(50)]
        public string? OfficeLetterNo { get; set; }

        [StringLength(50)]
        public string? AttachFileName { get; set; }

        public DateTime? EntryDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? StandardValue { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ContractValue { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TaxAmount { get; set; }

        [StringLength(50)]
        public string? PaymentRefID { get; set; }

        [StringLength(50)]
        public string? QRCodeNumber { get; set; }

        [StringLength(50)]
        public string? DemandNumber { get; set; }

        [StringLength(50)]
        public string? FormNumber { get; set; }

        [StringLength(50)]
        public string? TaxYear { get; set; }

        public bool? IsDeleted { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [ForeignKey("PersonalDetail")]
        [DisplayName("Personal Detail")]
        public int PersonalPkid { get; set; }
        public virtual PersonalDetail PersonalDetail { get; set; }

        [ForeignKey("Township")]
        [DisplayName("Person Township")]
        public int PersonTownshipPkid { get; set; }
        public virtual Township Township { get; set; }

        [NotMapped]
        public virtual VehicleStandardValue VehicleStandardValue { get; set; }
    }
}
