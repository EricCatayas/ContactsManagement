using System.ComponentModel.DataAnnotations;

namespace ContactsManagement.Core.Domain.Entities.ContactsManager
{
    /// <summary>
    /// Domain Model for Country
    ///     Do not expose to presentation layer; that includes Controller
    ///     i.e Neither as an arg nor a return type
    /// </summary>
    public class Country
    {
        [Key]
        public Guid CountryId { get; set; }
        [StringLength(50)]
        public string? CountryName { get; set; }
        public virtual ICollection<Person>? Persons { get; set; } //TODO : Remove

    }
}