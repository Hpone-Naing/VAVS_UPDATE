using System.ComponentModel;

namespace VAVS_Client.Models
{
    [Table("TB_Users")]
    public class User
    {
        [Key]
        public int UserPkid { get; set; }

        [Required(ErrorMessage = "UserID is required")]
        [StringLength(50, ErrorMessage = "UserID must be at most 50 characters")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password must be at most 50 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Display Name is required")]
        [StringLength(50, ErrorMessage = "Display Name must be at most 50 characters")]
        public string DisplayName { get; set; }

        [ForeignKey("UserType"), DisplayName("UserType")]
        public int UserTypeID { get; set; }
        public virtual UserType UserType { get; set; }

        public bool IsAuthenticateUser(string password)
        {
            return this.Password == password;
        }
    }
}
