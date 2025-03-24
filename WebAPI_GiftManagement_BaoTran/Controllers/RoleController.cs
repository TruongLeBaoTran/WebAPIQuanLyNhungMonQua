using Microsoft.AspNetCore.Mvc;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Services;
using static WebAPI_GiftManagement_BaoTran.Authorization.CustomAuthorizationAttribute;

namespace WebAPI_GiftManagement_BaoTran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [CustomAuthorize("ViewRole")]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(await _roleService.GetAllRoles());
        }

        [HttpGet("{idRole}")]
        [CustomAuthorize("ViewOneRole")]
        public async Task<IActionResult> GetSingleRole(int idRole)
        {
            RoleResponse role = await _roleService.GetSingleRole(idRole);
            if (role == null)
            {
                return BadRequest("Role not found");
            }
            return Ok(role);
        }

        [HttpPost]
        [CustomAuthorize("CreateRole")]
        public async Task<IActionResult> PostRole(RoleRequest role)
        {
            (bool success, string errorMessage) result = await _roleService.PostRole(role);
            if (!result.success)
            {
                return BadRequest(result.errorMessage);
            }

            return Ok("Data created successfully");
        }

        [HttpDelete("{id}")]
        [CustomAuthorize("DeleteRole")]
        public async Task<IActionResult> DeleteRole(int id)
        {

            (bool success, string errorMessage) result = await _roleService.DeleteRole(id);
            if (!result.success)
            {
                return BadRequest(result.errorMessage);
            }

            return Ok("Data deleted successfully");

        }

        [HttpPut("{id}")]
        [CustomAuthorize("UpdateRole")]
        public async Task<IActionResult> PutRole(int id, RoleRequest roleUpdate)
        {
            (bool success, string errorMessage) result = await _roleService.PutRole(id, roleUpdate);
            if (!result.success)
            {
                return BadRequest(result.errorMessage);
            }

            return Ok("Data updated successfully");
        }
    }
}
