using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalContact.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Mobile), IsUnique = true)]
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Person))]
        public int PersonId { get; set; }
        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }
        [Phone]
        [StringLength(15)]
        public string? Mobile { get; set; }
        [Phone]
        [StringLength(15)]
        public string? HomePhone { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        [StringLength(50)]
        public string? City { get; set; }
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        [ForeignKey(nameof(State))]
        public int StateId { get; set; }
        [StringLength(10)]
        public string? PostalCode { get; set; }
        public  Person? Person { get; set; }
        public Country? Country { get; set; }
        public State? State { get; set; }

    }
}
