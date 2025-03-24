using Microsoft.AspNetCore.Mvc;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Services;
using static WebAPI_GiftManagement_BaoTran.Authorization.CustomAuthorizationAttribute;

namespace WebAPI_GiftManagement_BaoTran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //Cấp xu
        [HttpPost("give-coins")]
        [CustomAuthorize("GiveCoins")]
        public async Task<IActionResult> GiveCoins(int idUser, int coins)
        {
            (bool Success, string ErrorMessage) result = await _userService.GiveCoins(idUser, coins);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok("Coins given successfully.");

        }


        [HttpGet]
        [CustomAuthorize("ViewUser")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userService.GetAllUsers());
        }


        [HttpGet("{userName}")]
        public async Task<IActionResult> GetSingleUser(string userName)
        {
            string? usernameClaim = User.Identity.Name;

            (bool Success, string ErrorMessage, UserResponse) result = await _userService.GetSingleUser(usernameClaim, userName);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Item3);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromForm] UserRequest userUpdate)
        {
            string? usernameClaim = User.Identity.Name;

            (bool Success, string ErrorMessage) result = await _userService.PutUser(usernameClaim, id, userUpdate);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok("Data updated successfully");

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            string? usernameClaim = User.Identity.Name;

            (bool Success, string ErrorMessage) result = await _userService.DeleteUser(usernameClaim, id);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Data deleted successfully");

        }

    }
}
