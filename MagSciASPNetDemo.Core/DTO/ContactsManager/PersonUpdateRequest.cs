using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Helpers;
using ContactsManagement.Core.Enums.ContactsManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations.Schema;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManagement.Core.DTO.ContactsManager
{
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "PersonId must be specified")]
        public Guid PersonId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Alpha characters only")]
        [MaxLength(100)]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }
        [DisplayName("Date of Birth")]
        [DateOfBirth(MinAge = 12, MaxAge = 100)]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public GenderOptions? Gender { get; set; }
        [Required(ErrorMessage = "CountryId is required")]
        public Guid? CountryId { get; set; }
        /* - The Hard Part - */
        [StringLength(75)]
        public string? JobTitle { get; set; }
        public int? CompanyId { get; set; }
        public string? ProfileBlobUrl { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? ContactNumber1 { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? ContactNumber2 { get; set; }
        public int? TagId { get; set; }
        public ContactTagDTO? Tag { get; set; }
        public List<int>? ContactGroups { get; set; }

        /// <summary>
        /// Converts the current object of PersonAddRequest into a Person object
        /// ContactTags, ContactGroups are to be handled in PersonUpdaterService separately
        /// ContactLogs in ContactLogsService
        /// </summary>
        /// <returns></returns>
        public Person ToPerson()
        {
            return new Person()
            {
                Id = PersonId,
                Name = PersonName,
                Email = Email,
                Address = Address,
                DateOfBirth = DateOfBirth,
                Gender = Enums.ContactsManager.Gender.Get(Gender),
                CountryId = CountryId,

                JobTitle = JobTitle,
                TagId = TagId,
                ContactNumber1 = ContactNumber1,
                ContactNumber2 = ContactNumber2,
                CompanyId = CompanyId,
                ProfileBlobUrl = ProfileBlobUrl,
                //ContactGroups                
            };
        }
    }
}
