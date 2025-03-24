using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_GiftManagement_BaoTran.Data
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCart { get; set; }

        public int IdMainGift { get; set; }
        public Gift Gift { get; set; }

        public int Quantity { get; set; }

        public int IdUser { get; set; }
        public User User { get; set; }

        public int TotalCoin { get; set; }
    }
}
