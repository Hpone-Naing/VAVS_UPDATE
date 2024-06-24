namespace VAVS_Client.Models
{
    [Table("TB_StateDivision")]
    public class StateDivision
    {
        [Key]
        public int StateDivisionPkid { get; set; }

        [Required]
        [StringLength(2)]
        public string? StateDivisionCode { get; set; }

        [StringLength(100)]
        public string? StateDivisionName { get; set; }

        [StringLength(100)]
        public string? CityOfRegion { get; set; }

        [StringLength(10)]
        public string? EngShortCode { get; set; }

        [StringLength(10)]
        public string? MynShortCode { get; set; }
    }
}
