using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [MaxLength(169)]
        public string Name { get; set; }

        public virtual ICollection<Town> Towns { get; set; } = new HashSet<Town>();
    }
}
