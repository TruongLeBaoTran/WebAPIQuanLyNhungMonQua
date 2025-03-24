using Microsoft.AspNetCore.Mvc;
using WebAPI_GiftManagement_BaoTran.Models;
using WebAPI_GiftManagement_BaoTran.Services;

namespace WebAPI_GiftManagement_BaoTran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankingController : ControllerBase
    {
        private readonly IRankingService rankingService;

        public RankingController(IRankingService rankingService)
        {
            this.rankingService = rankingService;
        }


        [HttpGet]
        //[CustomAuthorize("Cart")]
        public async Task<IActionResult> GetAll(int month, int year, int page)
        {

            (string Message, IEnumerable<RankingUserResponse>) result = await rankingService.GetRankingByTime(month, year, page);
            if (result.Item2 == null)
            {
                return BadRequest("No ranking");
            }
            return Ok(result.Item2);
        }

        [HttpPut("Approved")]
        // [CustomAuthorize("Cart")]
        public async Task<IActionResult> Approved(int idRanking)
        {
            (bool Success, string ErrorMessage) result = await rankingService.Approved(idRanking);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("Success");
        }

    }
}
