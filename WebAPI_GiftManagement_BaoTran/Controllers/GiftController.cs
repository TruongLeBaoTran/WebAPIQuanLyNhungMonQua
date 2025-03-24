using Microsoft.AspNetCore.Mvc;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Services;
using static WebAPI_GiftManagement_BaoTran.Authorization.CustomAuthorizationAttribute;

namespace WebAPI_GiftManagement_BaoTran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly IGiftService _giftService;

        public GiftController(IGiftService giftService)
        {
            _giftService = giftService;
        }


        [HttpGet]
        [CustomAuthorize("ViewGift")]
        public async Task<IActionResult> GetAllGift()
        {
            return Ok(await _giftService.GetAllGifts());
        }


        [HttpGet("{idGift}")]
        [CustomAuthorize("ViewOneGift")]
        public async Task<IActionResult> GetOneGift(int idGift)
        {
            GiftResponse gift = await _giftService.GetSingleGift(idGift);
            if (gift == null)
            {
                return BadRequest("Not found");
            }
            return Ok(gift);
        }


        [HttpPost]
        [CustomAuthorize("CreateGift")]

        public async Task<IActionResult> PostGift([FromForm] GiftRequest gift)
        {
            (bool Success, string ErrorMessage) result = await _giftService.PostGift(gift);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Data added successfully");
        }


        [HttpDelete("{id}")]
        [CustomAuthorize("DeleteGift")]
        public async Task<IActionResult> DeleteGift(int id)
        {
            (bool Success, string ErrorMessage) result = await _giftService.DeleteGift(id);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Data deleted successfully");
        }


        [HttpPut("{id}")]
        [CustomAuthorize("UpdateGift")]
        public async Task<IActionResult> PutGift(int id, [FromForm] GiftRequest userUpdate)
        {
            (bool Success, string ErrorMessage) result = await _giftService.PutGift(id, userUpdate);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Data updated successfully");
        }

    }
}
