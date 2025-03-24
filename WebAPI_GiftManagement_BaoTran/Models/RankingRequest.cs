namespace WebAPI_GiftManagement_BaoTran.Models
{
    public class RankingRequest
    {
        public int Month { get; set; }
        public int Year { get; set; }
        //public DateTime? ApprovedDate { get; set; }
        //public Boolean IsApproved { get; set; }
        public int IdUser { get; set; }
        public int Point { get; set; }
    }
}
