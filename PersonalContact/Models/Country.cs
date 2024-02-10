using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalContact.Models
{
    [Index(nameof(CountryName), IsUnique = true)]
    public class Country
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name ="Country")]       
        public string CountryName { get; set; }
        [Required]
        [StringLength(100)]
        public string Capital { get; set; }
        public virtual ICollection<State>? States { get; set; }
    }
}
