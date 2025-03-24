using Microsoft.AspNetCore.Mvc;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Services;
using static WebAPI_GiftManagement_BaoTran.Authorization.CustomAuthorizationAttribute;

namespace WebAPI_GiftManagement_BaoTran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        [CustomAuthorize("ViewAllPermissions")]
        public async Task<IActionResult> GetAllPermissions()
        {
            return Ok(await _permissionService.GetAllPermission());
        }

        [HttpGet("{idPermission}")]
        [CustomAuthorize("ViewOnePermission")]
        public async Task<IActionResult> GetSinglePermission(int idPermission)
        {
            PermissionResponse permission = await _permissionService.GetSinglePermission(idPermission);
            if (permission == null)
            {
                return BadRequest("Permission not found");
            }
            return Ok(permission);
        }

        [HttpPost]
        [CustomAuthorize("CreatePermission")]
        public async Task<IActionResult> PostPermission(PermissionRequest permission)
        {
            (bool success, string errorMessage) result = await _permissionService.PostPermission(permission);
            if (!result.success)
            {
                return BadRequest(result.errorMessage);
            }

            return Ok("Data created successfully");
        }

        [HttpDelete("{id}")]
        [CustomAuthorize("DeletePermission")]
        public async Task<IActionResult> DeletePermission(int id)
        {

            (bool success, string errorMessage) result = await _permissionService.DeletePermission(id);
            if (!result.success)
            {
                return BadRequest(result.errorMessage);
            }

            return Ok("Data deleted successfully");

        }

        [HttpPut("{id}")]
        [CustomAuthorize("UpdatePermission")]
        public async Task<IActionResult> PutPermission(int id, PermissionRequest permissionUpdate)
        {
            (bool success, string errorMessage) result = await _permissionService.PutPermission(id, permissionUpdate);
            if (!result.success)
            {
                return BadRequest(result.errorMessage);
            }

            return Ok("Data updated successfully");
        }
    }
}
