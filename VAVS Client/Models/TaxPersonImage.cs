using System.ComponentModel;

namespace VAVS_Client.Models
{
    [Table("TB_TaxPersonImage")]
    public class TaxPersonImage
    {
        [Key]
        public int TaxPersonImagePkid { get; set; }

        [MaxLength]
        public string? NrcImagePath { get; set; }

        [MaxLength]
        public string? CensusImagePath { get; set; }

        [MaxLength]
        public string? TransactionContractImagePath { get; set; }

        [MaxLength]
        public string? OwnerBookImagePath { get; set; }

        [MaxLength]
        public string? WheelTagImagePath { get; set; }

        [MaxLength]
        public string? VehicleImagePath { get; set; }

        [MaxLength]
        public string? NrcImageUrl { get; set; }

        [MaxLength]
        public string? CensusImageUrl { get; set; }

        [MaxLength]
        public string? TransactionContractImageUrl { get; set; }

        [MaxLength]
        public string? OwnerBookImageUrl { get; set; }

        [MaxLength]
        public string? WheelTagImageUrl { get; set; }

        [MaxLength]
        public string? VehicleImageUrl { get; set; }

        [StringLength(50)]
        public string? CarNumber { get; set; }

        public int? ImageId { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }

        public bool IsDeleted { get; set; }

        [ForeignKey("PersonalDetail")]
        [DisplayName("Personal Detail")]
        public int PersonalDetailPkid { get; set; }
        public virtual PersonalDetail PersonalDetail { get; set; }
    }
}
