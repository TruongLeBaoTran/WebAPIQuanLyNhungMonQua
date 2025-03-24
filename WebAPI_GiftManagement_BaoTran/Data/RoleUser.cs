namespace WebAPI_GiftManagement_BaoTran.Data
{
    public class RoleUser
    {
        public int IdRole { get; set; }
        public Role Role { get; set; }

        public int IdUser { get; set; }
        public User User { get; set; }

    }
}
