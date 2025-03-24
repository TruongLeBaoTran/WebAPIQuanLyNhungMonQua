using Microsoft.AspNetCore.Mvc;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Services;
using static WebAPI_GiftManagement_BaoTran.Authorization.CustomAuthorizationAttribute;

namespace WebAPI_GiftManagement_BaoTran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /*-----------------------------------------------------------------------
       ------Admin phát quà vào tủ đồ của 1 người (khi hết quà khuyến mãi)-------
       -------------------------------------------------------------------------*/
        [HttpPost("AddGiftWhenPromoGiftRunOut")]
        [CustomAuthorize("GivenGift")]
        public async Task<IActionResult> AddGiftWhenPromoGiftRunOut(TransactionDetailRequest addGiftToWardrobeGift)
        {
            (bool Success, string ErrorMessage) result = await _transactionService.AddGiftWhenPromoGiftRunOut(addGiftToWardrobeGift);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

        /*-----------------------------------------------------------------------
       ----Admin phát quà chính khi còn quà khuyến mãi (phát quà chính + km)-----
       -------------------------------------------------------------------------*/
        [HttpPost("AddGiftWhenPromoGiftAvailability")]
        [CustomAuthorize("GivenGift")]
        public async Task<IActionResult> AddGiftWhenPromoGiftAvailability(TransactionDetailRequest addGiftToWardrobeGift)
        {
            (bool Success, string ErrorMessage) result = await _transactionService.AddGiftWhenPromoGiftAvailability(addGiftToWardrobeGift);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

        /*-------------------------------------------------------------------------------------
        ------Tự động phát quà theo ngày được chỉ định sẵn, phát đến khi hết quà thì dừng------
        --------------------------------------------------------------------------------------*/
        [HttpPost("ScheduleGiftDistribution")]
        [CustomAuthorize("GivenGift")]
        public IActionResult ScheduleGiftDistribution(int idGift, int quantity, DateTime distributionTime, bool optionWhenPromoGiftAvailability)
        {
            string jobId = _transactionService.ScheduleGiftDistribution(idGift, quantity, distributionTime, optionWhenPromoGiftAvailability);
            return Ok(new { Success = true, JobId = jobId, Message = "Gift distribution scheduled successfully." });
        }

        [HttpDelete("cancel/{jobId}")]
        [CustomAuthorize("CancelScheduledGiftDistribution")]
        public IActionResult CancelScheduledGiftDistribution(string jobId)
        {
            bool success = _transactionService.CancelScheduledGiftDistribution(jobId);
            if (success)
            {
                return Ok(new { Success = true, Message = "Job canceled successfully." });
            }
            else
            {
                return BadRequest(new { Success = false, Message = "Failed to cancel job or job not found." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Purchase(int idUser)
        {
            string? usernameClaim = User.Identity.Name;
            (bool Success, string ErrorMessage) result = await _transactionService.Purchase(usernameClaim, idUser);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Success");
        }


        [HttpGet("PurchaseHistory")]
        public async Task<IActionResult> GetPurchaseHistory(int idUser)
        {
            string? usernameClaim = User.Identity.Name;
            (bool Success, string ErrorMessage, IEnumerable<TransactionResponse>) result = await _transactionService.GetPurchaseHistory(usernameClaim, idUser);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Item3);
        }

        [HttpGet("WinningGiftHistory")]
        public async Task<IActionResult> GetWinningGiftHistory(int idUser, int page)
        {
            string? usernameClaim = User.Identity.Name;
            (bool Success, string ErrorMessage, IEnumerable<IEnumerable<TransactionResponse>>) result = await _transactionService.GetWinningGiftHistory(idUser, page);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Item3);
        }

    }
}
