using System.ComponentModel;

namespace VAVS_Client.Models
{

    [Table("TB_Payment")]
    public class Payment
    {
        [Key]
        public int PaymentPkid { get; set; }

        [StringLength(50)]
        public string? TransactionNumber { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? PaidAmount { get; set; }

        public decimal? BalanceAmount { get; set; }

        [StringLength(50)]
        public string? TaxType { get; set; }

        [StringLength(100)]
        public string? AccountTitle { get; set; }

        [StringLength(50)]
        public string? PaymentType { get; set; }

        [StringLength(50)]
        public string? IncomeYear { get; set; }

        [StringLength(50)]
        public string? PaymentHubRefNo { get; set; }

        [StringLength(50)]
        public string? BankTransactionRefNo { get; set; }

        [StringLength(50)]
        public string? BankName { get; set; }

        [StringLength(50)]
        public string? PaymentMethod { get; set; }

        public DateTime? TransactionDate { get; set; }

        [StringLength(50)]
        public string? PaymentStatus { get; set; }

        public bool? IsDeleted { get; set; }

        [ForeignKey("PersonalDetail")]
        [DisplayName("Personal Detail")]
        public int PersonalPkid { get; set; }
        public virtual PersonalDetail PersonalDetail { get; set; }

        [ForeignKey("TaxValidation")]
        [DisplayName("Tax Validation")]
        public int TaxValidationPkid { get; set; }
        public virtual TaxValidation TaxValidation { get; set; }
    }
}
