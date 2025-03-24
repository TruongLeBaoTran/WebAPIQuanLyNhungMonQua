using Microsoft.AspNetCore.Mvc;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Services;

namespace WebAPI_GiftManagement_BaoTran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }


        [HttpGet("{idUser}")]
        //[CustomAuthorize("Cart")]
        public async Task<IActionResult> GetAllGiftInCart(int idUser)
        {
            string? usernameClaim = User.Identity.Name;

            (bool Success, string ErrorMessage, IEnumerable<CartResponse> listGiftInCart) result = await _cartService.GetAllGiftInCart(usernameClaim, idUser);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.listGiftInCart);
        }


        [HttpPost]
        //[CustomAuthorize("Cart")]
        public async Task<IActionResult> AddGiftToCart(CartRequest cart)
        {
            string? usernameClaim = User.Identity.Name;

            (bool Success, string ErrorMessage) result = await _cartService.AddGiftToCart(usernameClaim, cart);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Success");
        }


        [HttpPut("UpdateQuantity")]
        // [CustomAuthorize("Cart")]
        public async Task<IActionResult> UpdateQuantityGiftToCart(int idCartGift, int quantityRequest)
        {
            string? usernameClaim = User.Identity.Name;

            (bool Success, string ErrorMessage) result = await _cartService.UpdateQuantityGiftToCart(usernameClaim, idCartGift, quantityRequest);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Success");
        }


        [HttpPost("IncreaseOneGift")]
        //[CustomAuthorize("Cart")]
        public async Task<IActionResult> IncreaseOneGiftInCart(int idCartGift)
        {
            string? usernameClaim = User.Identity.Name;

            (bool Success, string ErrorMessage) result = await _cartService.IncreaseOneGiftInCart(usernameClaim, idCartGift);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Success");
        }


        [HttpPost("DecreaseOneGift")]
        // [CustomAuthorize("Cart")]
        public async Task<IActionResult> DecreaseOneGiftInCart(int idCartGift)
        {
            string? usernameClaim = User.Identity.Name;

            (bool Success, string ErrorMessage) result = await _cartService.DecreaseOneGiftInCart(usernameClaim, idCartGift);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Success");
        }


        [HttpPost("DeleteOneGift")]
        // [CustomAuthorize("Cart")]
        public async Task<IActionResult> DeleteOneGiftInCart(int idCartGift)
        {
            string? usernameClaim = User.Identity.Name;

            (bool Success, string ErrorMessage) result = await _cartService.DeleteOneGiftInCart(usernameClaim, idCartGift);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Success");
        }


        [HttpPost("DeleteAllGift")]
        // [CustomAuthorize("Cart")]
        public async Task<IActionResult> DeleteAllGiftInCart(int idCartGift)
        {
            string? usernameClaim = User.Identity.Name;

            (bool Success, string ErrorMessage) result = await _cartService.DeleteAllGiftInCart(usernameClaim, idCartGift);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Success");
        }
    }
}
