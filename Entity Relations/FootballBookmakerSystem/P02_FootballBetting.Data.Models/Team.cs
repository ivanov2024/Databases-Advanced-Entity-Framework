using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        [Required]
        public string LogoUrl { get; set; }

        [Required]
        [MaxLength(4)]
        public string Initials { get; set; }

        [Required]
        public decimal Budget {  get; set; }

        [ForeignKey(nameof(PrimaryKitColor))]
        public int PrimaryKitColorId { get; set; }

        public virtual Color PrimaryKitColor { get; set; }

        [ForeignKey(nameof(SecondaryKitColor))]
        public int SecondaryKitColorId { get; set; }

        public virtual Color SecondaryKitColor { get; set; }

        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        public virtual ICollection<Game> HomeGames { get; set; } = new HashSet<Game>();
        public virtual ICollection<Game> AwayGames { get; set; } = new HashSet<Game>();
        public virtual ICollection<Player> Players { get; set; } = new HashSet<Player>();
    }
}
