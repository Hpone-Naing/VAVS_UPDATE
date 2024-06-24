namespace VAVS_Client.Models
{
    [Table("TB_UserTypes")]
    public class UserType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserTypeID { get; set; }

        [StringLength(50)]
        public string UserTypeName { get; set; }
    }
}
