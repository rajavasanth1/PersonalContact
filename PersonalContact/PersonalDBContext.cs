using Microsoft.EntityFrameworkCore;
using PersonalContact.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PersonalContact
{
    public class PersonalDBContext : DbContext
    {
        public PersonalDBContext(DbContextOptions<PersonalDBContext> options) : base(options)
        {

        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Contact> Contact { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<State>().ToTable("States");

            modelBuilder.Entity<Country>().HasData(new Country { Id = 1, CountryName = "India", Capital = "New Delhi" });
            modelBuilder.Entity<Country>().HasIndex(country => country.CountryName).IsUnique();
        }
        public DbSet<PersonalContact.Models.State> State { get; set; } = default!;
    }
}
