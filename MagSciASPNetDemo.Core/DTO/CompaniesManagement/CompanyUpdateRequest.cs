using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.DTO.CompaniesManagement
{
    public class CompanyUpdateRequest
    {
        [Required]
        public int CompanyId { get; set; }
        [Required(ErrorMessage = "Company name is required")]
        [DisplayName("Company Name")]
        [MaxLength(100, ErrorMessage = "Company name must not exceed 100 characters")]
        public string CompanyName { get; set; }
        [StringLength(500, ErrorMessage = "Company description must not exceed 500 characters")]
        public string? CompanyDescription { get; set; }
        [StringLength(50, ErrorMessage = "Industry must not exceed 50 characters")]
        public string? Industry { get; set; }
        [StringLength(100, ErrorMessage = "Address1 must not exceed 100 characters")]
        public string? Address1 { get; set; }
        [StringLength(100, ErrorMessage = "Address2 must not exceed 100 characters")]
        public string? Address2 { get; set; }
        [EmailAddress]
        [DisplayName("Contact Email")]
        public string? ContactEmail { get; set; }
        [DataType(DataType.Url)]
        public string? WebUrl { get; set; }
        [StringLength(30, ErrorMessage = "Contact number must not exceed 30 characters")]
        [DataType(DataType.PhoneNumber)]
        public string? ContactNumber1 { get; set; }
        [StringLength(30, ErrorMessage = "Contact number must not exceed 30 characters")]
        [DataType(DataType.PhoneNumber)]
        public string? ContactNumber2 { get; set; }
        public List<Guid>? Employees { get; set; }  // Search Feature for this -- input[type="checkbox"]
        public Guid? UserId { get; set; }

        public Company ToCompany()
        {
            return new Company()
            {
                CompanyId = CompanyId,
                CompanyName = CompanyName,
                CompanyDescription = CompanyDescription,
                Industry = Industry,
                Address1 = Address1,
                Address2 = Address2,
                ContactEmail = ContactEmail,
                ContactNumber1 = ContactNumber1,
                ContactNumber2 = ContactNumber2,
                WebUrl = WebUrl,
                UserId = UserId
                //Employees
            };
        }
    }
}
