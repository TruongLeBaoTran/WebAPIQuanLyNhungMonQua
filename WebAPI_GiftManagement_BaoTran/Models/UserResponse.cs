namespace WebAPI_GiftManagement_BaoTran.Models
{
    public class UserResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Image { get; set; }
        public DateTime? Birthday { get; set; }
        public int Age => CalculateAge(Birthday);


        private static int CalculateAge(DateTime? birthDate)
        {
            if (!birthDate.HasValue)
            {
                return 0;
            }

            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Value.Year;

            if (birthDate.Value.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}