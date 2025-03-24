namespace WebAPI_GiftManagement_BaoTran.Models
{
    public class GiftResponse
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public int? Coin { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public int RemainingQuantity { get; set; }

        public int IdCategory { get; set; }
        public int? AccumulatedPoints { get; set; }

    }
}
