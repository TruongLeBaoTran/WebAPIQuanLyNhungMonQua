using Microsoft.AspNetCore.Mvc;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Services;
using static WebAPI_GiftManagement_BaoTran.Authorization.CustomAuthorizationAttribute;

namespace WebAPI_GiftManagement_BaoTran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionController : ControllerBase
    {
        private readonly IRolePermissionService _rolePermissionService;
        public RolePermissionController(IRolePermissionService rolepermissionService)
        {
            _rolePermissionService = rolepermissionService;
        }

        [HttpPost]
        [CustomAuthorize("AddListPermissionToRole")]
        public async Task<IActionResult> AddListPermissionToRole(int idRole, List<int> listIdPermissions)
        {
            (bool Success, string ErrorMessage) result = await _rolePermissionService.AddListPermissionToRole(idRole, listIdPermissions);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Success");
        }

        [HttpGet("{idRole}")]
        [CustomAuthorize("GetPermissionInRole")]
        public async Task<IActionResult> GetPermissionInRole(int idRole)
        {
            IEnumerable<RolePermissionResponse> listPermission = await _rolePermissionService.GetPermissionInRole(idRole);
            if (listPermission == null)
            {
                return BadRequest("Role has no permissions");
            }
            return Ok(listPermission);
        }
    }
}
