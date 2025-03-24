using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebAPI_GiftManagement_BaoTran.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement> //PermissionHandler sẽ xử lý các yêu cầu phân quyền dựa trên permissions.
    {
        private readonly IUserPermissions _permissionService;

        public PermissionHandler(IUserPermissions permissionService)
        {
            _permissionService = permissionService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            Claim? userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return;
            }

            // Lấy danh sách permissions của user từ database
            List<Data.Permission> userPermissions = await _permissionService.GetUserPermissionsAsync(userId);

            //Kiểm tra nếu user có quyền cần thiết
            if (userPermissions != null && userPermissions.Any(p => p.Code == requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }




}



