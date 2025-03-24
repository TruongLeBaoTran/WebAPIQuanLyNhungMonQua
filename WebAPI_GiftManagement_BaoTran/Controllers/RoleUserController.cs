using Microsoft.AspNetCore.Mvc;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Services;
using static WebAPI_GiftManagement_BaoTran.Authorization.CustomAuthorizationAttribute;

namespace WebAPI_GiftManagement_BaoTran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleUserController : ControllerBase
    {
        private readonly IRoleUserService _roleUserService;
        public RoleUserController(IRoleUserService rolepermissionService)
        {
            _roleUserService = rolepermissionService;
        }

        [HttpPost]
        [CustomAuthorize("AddListUserToRole")]
        public async Task<IActionResult> AddListUserToRole(int idRole, List<int> listIdUsers)
        {
            (bool Success, string ErrorMessage) result = await _roleUserService.AddListUserToRole(idRole, listIdUsers);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Success");
        }

        [HttpGet("{idRole}")]
        [CustomAuthorize("ViewUserInRole")]
        public async Task<IActionResult> GetUserInRole(int idRole)
        {
            IEnumerable<RoleUserResponse> listUser = await _roleUserService.GetUserInRole(idRole);
            if (listUser == null)
            {
                return BadRequest("Role has no users");
            }
            return Ok(listUser);
        }
    }
}
