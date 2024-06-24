namespace VAVS_Client.Models
{
    [Table("TB_FuelType")]
    public class Fuel
    {
        [Key]
        public int FuelTypePkid { get; set; }

        [StringLength(20)]
        public string? FuelType { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
