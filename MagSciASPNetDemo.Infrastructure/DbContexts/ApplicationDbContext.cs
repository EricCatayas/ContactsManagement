using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ContactsManagement.Infrastructure.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        /// <summary>
        /// Whatever is supplied to options arg i.e .AddService(), will be supplied to this ctor and parent class
        /// Nuget: EntityFramework.Tools   EntityFramework.Design    EntityFramework.SqlServer
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<ContactGroup> ContactGroups { get; set; }
        public virtual DbSet<ContactTag> ContactTags { get; set; }
        public virtual DbSet<ContactLog> ContactLogs { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        /* -- Contacts Management Expanded -- */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // So any code changes to OnModelCreating() will be reflected on the Migration
            base.OnModelCreating(modelBuilder);

            // See? Id is auto generated baby
            modelBuilder.Entity<Country>()
               .Property(temp => temp.CountryId)
               .ValueGeneratedOnAdd();
            modelBuilder.Entity<Company>()
                .Property(temp => temp.CompanyId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<ContactGroup>()
                .Property(temp => temp.GroupId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<ContactTag>()
                .Property(temp => temp.TagId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<ContactLog>()
                .Property(temp => temp.LogId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Event>()
                .Property(temp => temp.EventId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");
            modelBuilder.Entity<ContactTag>().ToTable("ContactTags");
            modelBuilder.Entity<ContactGroup>().ToTable("ContactGroups");
            modelBuilder.Entity<ContactLog>().ToTable("ContactLogs");
            modelBuilder.Entity<Company>().ToTable("Companies");
            modelBuilder.Entity<Event>().ToTable("Events");


            //Countries Seed using Fluent API
           /* string countriesJson = File.ReadAllText("seedData/countries.json");
            List<Country>? countriesSeedData = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);
            countriesSeedData.ForEach(country =>
            {
                modelBuilder.Entity<Country>().HasData(country);
            });*/       
            // But fluent api is too explicit
        }
        /* ------------ Stored Procedure Sample ------------ */
        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }
        public int sp_InsertPerson(Person person)
        {
            // MUST MATCH STORED PROCEDURE SEQUENCE
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id",person.Id),
                new SqlParameter("@Name",person.Name),
                new SqlParameter("@Email",person.Email),
                new SqlParameter("@DateOfBirth",person.DateOfBirth),
                new SqlParameter("@Gender",person.Gender),
                new SqlParameter("@CountryId",person.Country.CountryId),
                new SqlParameter("@Address",person.Address),
            };
            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @Id, @Name, @Email, @DateOfBirth, @Gender, @CountryId, @Address", parameters);
        }
        public int sp_UpdatePerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id",person.Id),
                new SqlParameter("@Name",person.Name),
                new SqlParameter("@Email",person.Email),
                new SqlParameter("@DateOfBirth",person.DateOfBirth),
                new SqlParameter("@Gender",person.Gender),
                new SqlParameter("@CountryId",person.Country.CountryId),
                new SqlParameter("@Address",person.Address),
            };
            return Database.ExecuteSqlRaw("EXECUTE [dbo].[UpdatePerson] @Id, @Name, @Email, @DateOfBirth, @Gender, @CountryId, @Address", parameters);

        }
    }
}
