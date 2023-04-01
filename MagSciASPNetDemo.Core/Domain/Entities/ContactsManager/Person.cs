using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.Entities.ContactsManager
{
    /// <summary>
    /// Person domain model class
    /// </summary>
    public class Person
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string? Name { get; set; }
        [StringLength(30)]
        public string? Email { get; set; }
        [StringLength(100)]
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(15)]
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country? Country { get; set; }

        /* - The Fun Part - */
        [StringLength(75)]
        public string? JobTitle { get; set; }
        [StringLength(30)]
        public string? ContactNumber1 { get; set; }
        [StringLength(30)]
        public string? ContactNumber2 { get; set; }
        /* - Contacts Management Expansion - */
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }
        [MaxLength(1028)]
        public string? ProfileBlobUrl { get; set; }
        public int? TagId { get; set; }
        [ForeignKey("TagId")]
        public virtual ContactTag? Tag { get; set; }
        public virtual ICollection<ContactGroup>? ContactGroups { get; set; }
        public virtual ICollection<ContactLog>? ContactLogs { get; set; }
    }
}
