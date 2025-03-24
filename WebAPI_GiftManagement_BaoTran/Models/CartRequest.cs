namespace WebAPI_GiftManagement_BaoTran.Models
{
    public class CartRequest
    {
        public int IdMainGift { get; set; }

        public int Quantity { get; set; }

        public int IdUser { get; set; }
    }
}
