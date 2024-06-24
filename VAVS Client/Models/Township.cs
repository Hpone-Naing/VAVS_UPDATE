using System.ComponentModel;

namespace VAVS_Client.Models
{
    [Table("TB_Township")]
    public class Township
    {
        [Key]
        public int TownshipPkid { get; set; }

        [Required]
        [StringLength(15)]
        public string? TownshipCode { get; set; }

        [StringLength(100)]
        public string? TownshipName { get; set; }

        [StringLength(5)]
        public string? DistrictCode { get; set; }

        [ForeignKey("StateDivision")]
        [DisplayName("State Division")]
        public int StateDivisionPkid { get; set; }
        public virtual StateDivision StateDivision { get; set; }

        [StringLength(5)]
        public string? StateDivisionID { get; set; }
    }
}
