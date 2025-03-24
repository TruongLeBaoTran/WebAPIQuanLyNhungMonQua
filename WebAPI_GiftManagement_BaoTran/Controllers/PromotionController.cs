using Microsoft.AspNetCore.Mvc;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Services;
using static WebAPI_GiftManagement_BaoTran.Authorization.CustomAuthorizationAttribute;

namespace WebAPI_GiftManagement_BaoTran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            this.promotionService = promotionService;
        }

        [HttpPost]
        [CustomAuthorize("AddPromoGiftToGift")]
        public async Task<IActionResult> AddPromoGiftToGift(PromotionRequest gift_PromoGiftRequest)
        {
            (bool Success, string ErrorMessage) result = await promotionService.AddPromoGiftToGift(gift_PromoGiftRequest);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Success");
        }

        [HttpGet("{idGift}")]
        [CustomAuthorize("ViewPromoGiftToGift")]
        public async Task<IActionResult> GetPromoGiftInGift(int idGift)
        {
            IEnumerable<PromotionResponse> listPromoGift = await promotionService.GetPromoGiftInGift(idGift);
            if (listPromoGift == null)
            {
                return BadRequest("Gift has no promo gift");
            }
            return Ok(listPromoGift);
        }

        [HttpPut("{id}")]
        [CustomAuthorize("UpdateQuantityPromoGiftInGift")]
        public async Task<IActionResult> PutQuantityPromoGiftInGift(PromotionRequest gift_PromoGiftRequest)
        {
            (bool Success, string ErrorMessage) result = await promotionService.PutQuantityPromoGiftInGift(gift_PromoGiftRequest);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Data updated successfully");
        }


        [HttpDelete("{idPromotion}")]
        [CustomAuthorize("DeletePromoGiftInGift")]
        public async Task<IActionResult> DeletePromoGiftInGift(int idPromotion)
        {
            (bool Success, string ErrorMessage) result = await promotionService.DeletePromoGiftInGift(idPromotion);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Data deleted successfully");
        }

    }
}
