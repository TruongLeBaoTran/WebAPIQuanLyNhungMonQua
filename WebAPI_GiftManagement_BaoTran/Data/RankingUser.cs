using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_GiftManagement_BaoTran.Data
{
    public class RankingUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int IdRanking { get; set; }
        public Ranking Ranking { get; set; }

        public int IdUser { get; set; }
        public User User { get; set; }

        public int Point { get; set; }

    }
}
