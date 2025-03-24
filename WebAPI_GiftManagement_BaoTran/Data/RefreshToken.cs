using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_GiftManagement_BaoTran.Data
{
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int IdUser { get; set; }

        [ForeignKey(nameof(IdUser))]
        public User User { get; set; }

        public string Token { get; set; }

        public string JwtId { get; set; }

        public bool IsUsed { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime ExpiredAt { get; set; }
    }
}
