using Microsoft.AspNetCore.Mvc;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Services;
using static WebAPI_GiftManagement_BaoTran.Authorization.CustomAuthorizationAttribute;

namespace WebAPI_GiftManagement_BaoTran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService loginService, IUserService userService)
        {
            _authService = loginService;
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(Login model)
        {
            (bool Success, string ErrorMessage, Token token) result = await _authService.LoginUser(model);

            if (!result.Success)
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = result.ErrorMessage
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Authentication success",
                Data = result.token
            });
        }

        [HttpPost("LoginAdmin")]
        public async Task<IActionResult> LoginAdmin(Login model)
        {

            (bool Success, string ErrorMessage, Token token) result = await _authService.LoginAdmin(model);

            if (!result.Success)
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = result.ErrorMessage
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Authentication success",
                Data = result.token
            });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] UserRequest userNew)
        {
            (bool Success, string ErrorMessage) = await _authService.Register(userNew);

            if (Success)
            {
                return Ok("Data added successfully");
            }

            return BadRequest(ErrorMessage);
        }

        [HttpPost("CreateAdmin")]
        [CustomAuthorize("CreateAdmin")]
        public async Task<IActionResult> CreateAdmin([FromForm] UserRequest userNew)
        {
            (bool Success, string ErrorMessage) = await _authService.CreateAdmin(userNew);

            if (Success)
            {
                return Ok("Data added successfully");
            }

            return BadRequest(ErrorMessage);
        }

        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(Token token)
        {
            (bool Success, string ErrorMessage, Token token) result = await _authService.RenewToken(token);

            if (!result.Success)
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = result.ErrorMessage
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Token renewed successfully",
                Data = result.token
            });
        }
    }
}
