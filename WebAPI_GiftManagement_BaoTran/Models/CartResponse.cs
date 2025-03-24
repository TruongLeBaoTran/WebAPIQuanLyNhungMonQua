
namespace WebAPI_GiftManagement_BaoTran.Models
{
    public class CartResponse
    {
        public int IdMainGift { get; set; }

        public int Quantity { get; set; }

        public string GiftName { get; set; }

        public string GiftImage { get; set; }

        public int? GiftCoin { get; set; }

        public IEnumerable<PromoGiftResponse> ListPromoGift { get; set; } //ds quà km
    }
}
