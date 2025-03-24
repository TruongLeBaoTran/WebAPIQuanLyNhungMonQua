namespace WebAPI_GiftManagement_BaoTran.Models
{
    public class TransactionDetailRequest
    {
        public int IdUser { get; set; }
        public int IdMainGift { get; set; }
        public int Quantity { get; set; }
    }
}
