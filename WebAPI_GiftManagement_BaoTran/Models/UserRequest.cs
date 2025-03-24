namespace WebAPI_GiftManagement_BaoTran.Models
{
    public class UserRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTime? Birthday { get; set; }
        public IFormFile? Image { get; set; }

    }
}
