using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_GiftManagement_BaoTran.Data
{
    public class Gift
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdGift { get; set; }

        public string Name { get; set; }

        public int? Coin { get; set; }

        public string? Image { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public int RemainingQuantity { get; set; }
        public int IdCategory { get; set; }
        public Category Category { get; set; }

        public int? AccumulatedPoints { get; set; }

        public ICollection<Promotion> MainGifts { get; set; }
        public ICollection<Promotion> PromoGifts { get; set; }
        public ICollection<TransactionDetail> TransactionDetails { get; set; }
        public ICollection<Cart> Carts { get; set; }



    }
}
