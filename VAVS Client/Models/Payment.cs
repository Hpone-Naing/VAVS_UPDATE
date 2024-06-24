namespace VAVS_Client.Models
{
    [Table("TB_Payment")]
    public class Payment
    {
        [Key]
        public int PaymentPkid { get; set; }
        public DateTime PaymentDate { get; set; }

        [StringLength(20)]
        public string PaymentType { get; set; }

        [StringLength(20)]
        public string PaymentAmount { get; set; }
    }
}
