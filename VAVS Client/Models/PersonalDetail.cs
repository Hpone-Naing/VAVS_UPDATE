using System.ComponentModel;

namespace VAVS_Client.Models
{
    [Table("TB_PersonalDetail")]
    public class PersonalDetail
    {
        [Key]
        public int PersonalPkid { get; set; }

        [StringLength(50)]
        public string? TransactionID { get; set; }
        public DateTime? EntryDate { get; set; }

        [StringLength(30)]
        public string? PersonTINNumber { get; set; }

        [StringLength(30)]
        public string? GIR { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(50)]
        public string? NRCTownshipNumber { get; set; }

        [StringLength(50)]
        public string? NRCTownshipInitial { get; set; }

        [StringLength(50)]
        public string? NRCType { get; set; }

        [StringLength(50)]
        public string? NRCNumber { get; set; }

        [MaxLength(200)]
        public string? NRCFrontImagePath { get; set; }

        [MaxLength(200)]
        public string? NRCFrontImageUrl { get; set; }

        [NotMapped]
        [DisplayName("မှတ်ပုံတင်‌ရှေ့ပိုင်းပုံထည့်ရန်")]
        public IFormFile? NrcFrontImageFile { get; set; }

        [MaxLength(200)]
        public string? NRCBackImagePath { get; set; }

        [MaxLength(200)]
        public string? NRCBackImageUrl { get; set; }

        [NotMapped]
        [DisplayName("မှတ်ပုံတင်‌နောက်ပိုင်းပုံထည့်ရန်")]
        public IFormFile NrcBackImageFile { get; set; }

        [StringLength(100)]
        public string? Quarter { get; set; }

        [StringLength(200)]
        public string? Street { get; set; }

        [StringLength(200)]
        public string? HousingNumber { get; set; }

        [StringLength(50)]
        public string? PhoneNumber { get; set; }

        [StringLength(50)]
        public string? Email { get; set; }

        

        [StringLength(15)]
        public string? RegistrationStatus { get; set; }

        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }

        [ForeignKey("StateDivision")]
        [DisplayName("State Division")]
        public int StateDivisionPkid { get; set; }
        public virtual StateDivision StateDivision { get; set; }

        [ForeignKey("Township")]
        [DisplayName("Township")]
        public int TownshipPkid { get; set; }
        public virtual Township Township { get; set; }
        public string MakeNrc() => String.Concat(this.NRCTownshipNumber, ";", this.NRCTownshipInitial, ";", this.NRCType, ";", this.NRCNumber);
        
        public string MakePhoneNumberWithCountryCode()
        {
            int index = PhoneNumber.IndexOf("09");
            if (index != -1)
            {
                return PhoneNumber.Substring(0, index) + "+959" + PhoneNumber.Substring(index + 2);
            }
            else
            {
                return PhoneNumber;
            }
        }
    }
}
