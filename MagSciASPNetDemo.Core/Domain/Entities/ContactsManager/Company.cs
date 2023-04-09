using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.Entities.ContactsManager
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        [Required]
        [StringLength(100)]
        public string CompanyName { get;  set; }
        [StringLength(500)]
        public string? CompanyDescription { get; set; }
        [StringLength(50)]
        public string? Industry { get; set; } 
        [StringLength(100)]
        public string? Address1 { get; set; }
        [StringLength(100)]
        public string? Address2 { get; set; }
        [EmailAddress]
        [StringLength(50)]
        public string? ContactEmail { get; set; }
        [MaxLength(1028)]
        [DataType(DataType.Url)]
        public string? WebUrl { get; set; }
        [StringLength(30)]
        public string? ContactNumber1 { get; set; }
        [StringLength(30)]
        public string? ContactNumber2 { get; set; }
        public virtual ICollection<Person>? Employees { get; set; }
        public Guid? UserId { get; set; }
    }
}
