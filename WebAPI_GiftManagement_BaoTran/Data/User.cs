using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_GiftManagement_BaoTran.Data
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime? Birthday { get; set; }

        public string? Image { get; set; }

        public int? Coin { get; set; }

        public ICollection<RoleUser> RoleUsers { get; set; } = new List<RoleUser>();

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public ICollection<Cart> Carts { get; set; } = new List<Cart>();

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public ICollection<RankingUser> RankingUsers { get; set; } = new List<RankingUser>();

    }
}
