using VAVS_Client.Util;

namespace VAVS_Client.Models
{
    [Table("TB_SearchLimit")]
    public class SearchLimit
    {
        [Key]
        public int SearchLimitPkid { get; set; }

        [StringLength(50)]
        public string? Nrc { get; set; }

        public int SearchCount { get; set; }

        [StringLength(200)]
        public string? ReSearchTime { get; set; }

        public bool IsExceedMaximunSearch() => (this.SearchCount >= Utility.MAXIMUM_SEARCH_TIME);

        public bool AllowNextTimeRegiste()
        {
            return (this.ReSearchTime != null && DateTime.Now >= DateTime.Parse(this.ReSearchTime));
        }
    }
}
