namespace VAVS_Client.Models
{
    [Table("TB_NRC_And_Township")]
    public class NRC_And_Township
    {
        [Key]
        public int NRC_And_Township_Pkid { get; set; }

        [StringLength(50)]
        public string? NrcInitialCodeEnglish { get; set; }

        [StringLength(50)]
        public string? NrcInitialCodeMyanmar { get; set; }

        [StringLength(50)]
        public string? NrcTownshipCodeEng { get; set; }

        [StringLength(50)]
        public string? NrcTownshipCodeMyn { get; set; }

        [StringLength(100)]
        public string? PresentTownship { get; set; }

        [StringLength(2)]
        public string? TownshipDigitCode { get; set; }

        public bool? IsDeleted { get; set; }

    }
}
