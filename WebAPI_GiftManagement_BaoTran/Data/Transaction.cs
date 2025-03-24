using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_GiftManagement_BaoTran.Data
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTransaction { get; set; }

        public int IdUser { get; set; }
        public User User { get; set; }

        public DateTime TransactionTime { get; set; }

        public int CoinTotal { get; set; }

        public int? AccumulatedPoints { get; set; }

        public ICollection<TransactionDetail> TransactionDetails { get; set; }


    }
}
