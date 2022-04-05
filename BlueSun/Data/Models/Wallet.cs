namespace BlueSun.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class Wallet
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public User User { get; set;  }

        [Column(TypeName = "decimal(9,2)")]
        public decimal Balance { get; set; }
    }
}
