using Microsoft.AspNetCore.Authorization;

namespace WebAPI_GiftManagement_BaoTran.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }


}
