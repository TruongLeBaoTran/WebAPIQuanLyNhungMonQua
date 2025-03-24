using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI_GiftManagement_BaoTran.Authorization
{
    public class CustomAuthorizationAttribute
    {
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
        public class CustomAuthorizeAttribute : TypeFilterAttribute
        {
            public CustomAuthorizeAttribute(string permission) : base(typeof(PermissionFilter))
            {
                Arguments = new object[] { new PermissionRequirement(permission) };
            }
        }

        public class PermissionFilter : IAuthorizationFilter //là nơi thực hiện việc kiểm tra quyền truy cập dựa trên quyền (permission) được chỉ định.
        {
            private readonly IAuthorizationService _authorization;
            private readonly PermissionRequirement _requirement;

            public PermissionFilter(IAuthorizationService authorization, PermissionRequirement requirement)
            {
                _authorization = authorization;
                _requirement = requirement;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                AuthorizationResult result = _authorization.AuthorizeAsync(context.HttpContext.User, null, _requirement).Result;
                if (!result.Succeeded)
                {
                    context.Result = new ForbidResult();
                }
            }
        }

    }
}
