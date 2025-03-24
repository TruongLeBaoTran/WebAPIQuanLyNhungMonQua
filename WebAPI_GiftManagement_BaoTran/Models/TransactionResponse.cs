namespace WebAPI_GiftManagement_BaoTran.Models
{
    public class TransactionResponse
    {
        public DateTime TransactionTime { get; set; }
        public int CoinTotal { get; set; }
        public int IdGift { get; set; }
        public string GiftName { get; set; }
        public string GiftImage { get; set; }
        public int? GiftCoin { get; set; }
        public int? MainGift { get; set; }
        public int Quantity { get; set; }
        public int AccumulatedPoints { get; set; }

    }
}
