using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_GiftManagement_BaoTran.Data
{
    public class Ranking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRanking { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public Boolean IsApproved { get; set; }
        public ICollection<RankingUser> RankingUsers { get; set; } = new List<RankingUser>();

    }
}
