using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.Helpers;
using ContactsManagement.Core.Domain.Entities.ContactsManager;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManagement.Core.DTO.ContactsManager
{
    /// <summary>
    /// DTO for updating a person object
    /// </summary>
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [DisplayName("Name")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Alpha characters only")]
        [MaxLength(100)]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        public string? Address { get; set; }
        [DisplayName("Date of Birth")]
        [DateOfBirth(MinAge = 12, MaxAge = 100)]
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryId { get; set; }
        /* - The Hard Part - */
        [StringLength(75)]
        public string? JobTitle { get; set; }
        [DisplayName("Contact Number 1")]
        [DataType(DataType.PhoneNumber)]
        public string? ContactNumber1 { get; set; }
        [DisplayName("Contact Number 2")]
        [DataType(DataType.PhoneNumber)]
        public string? ContactNumber2 { get; set; }
        /* - Panza - */
        public int? CompanyId { get; set; }
        [DisplayName("Tag")]
        public int? TagId { get; set; }
        [DisplayName("Contact Groups")]
        public List<int>? ContactGroups { get; set; }
        public string? ProfileBlobUrl { get; set; }
        /// <summary>
        /// Converts the current object of PersonAddRequest into a Person object
        /// </summary>
        /// <returns></returns>
        public Person ToPerson()
        {
            return new Person()
            {
                Id = Guid.NewGuid(),
                Name = PersonName,
                Email = Email,
                Address = Address,
                DateOfBirth = DateOfBirth,
                CountryId = (Guid)CountryId,
                Gender = Gender.ToString(),
                
                JobTitle = JobTitle,
                ContactNumber1 = ContactNumber1,
                ContactNumber2 = ContactNumber2,
                CompanyId = CompanyId,
                TagId = TagId,
                ProfileBlobUrl = ProfileBlobUrl
            };
        }
    }
}
