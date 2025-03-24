using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_GiftManagement_BaoTran.Data
{
    public class Promotion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPromotion { get; set; }

        public int IdMainGift { get; set; }
        public Gift MainGift { get; set; }

        public int IdPromoGift { get; set; }
        public Gift PromoGift { get; set; }

        public int Quantity { get; set; }


    }
}
