using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_GiftManagement_BaoTran.Data
{
    public class TransactionDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int IdTransaction { get; set; }
        public Transaction Transaction { get; set; }

        public int IdGift { get; set; }
        public Gift Gift { get; set; }

        public int? MainGift { get; set; }

        public int Quantity { get; set; }
    }
}
