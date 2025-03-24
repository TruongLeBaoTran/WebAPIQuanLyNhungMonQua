using System.ComponentModel.DataAnnotations;

namespace WebAPI_GiftManagement_BaoTran.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
