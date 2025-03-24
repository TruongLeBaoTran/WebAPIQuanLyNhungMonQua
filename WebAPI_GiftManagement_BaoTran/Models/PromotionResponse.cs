namespace WebAPI_GiftManagement_BaoTran.Models
{
    public class PromotionResponse
    {
        public int IdMainGift { get; set; }

        public string MainGiftName { get; set; }

        public string MainGiftImage { get; set; }

        public int IdPromoGift { get; set; }

        public string PromoGiftName { get; set; }

        public string PromoGiftImage { get; set; }

        public int Quantity { get; set; }
    }
}
