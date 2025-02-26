using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string Password { get; set; }

        [Required]
        [MaxLength(412)]
        public string Email { get; set; }

        [Required]
        public decimal Balance { get; set; }

        public virtual ICollection<Bet> Bets { get; set; } = new HashSet<Bet>();
    }
}
