namespace WebAPI_GiftManagement_BaoTran.Data
{
    public class RolePermission
    {
        public int IdRole { get; set; }
        public Role Role { get; set; }

        public int IdPermission { get; set; }
        public Permission Permission { get; set; }
    }
}
