using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ContactsManagement.Core.DTO.ContactsManager
{
    /// <summary>
    /// DTO class return type for IPersonService converted from Person model class
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        [DateOfBirth(MinAge = 12, MaxAge = 100)]
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public string? CountryName { get; set; }
        public Guid? CountryId { get; set; }
        public int? Age
        {
            get
            {
                return DateOfBirth != null ? (int)Math.Floor((DateTime.Now - DateOfBirth.Value).TotalDays / 365.25) : null;
            }
        }
        /* - The Hard Part - */
        public string? JobTitle { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactNumber1 { get; set; }
        public string? ContactNumber2 { get; set; }
        public int? TagId { get; set; }
        public ContactTagDTO? Tag { get; set; }
        public List<ContactGroupResponse>? ContactGroups { get; set; }
        public string? ProfileBlobUrl { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse other = (PersonResponse)obj;
            return other.PersonId == PersonId ? true : false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public static class PersonResponseExtentions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonId = person.Id,
                PersonName = person.Name,
                Email = person.Email,
                Address = person.Address,
                DateOfBirth = person.DateOfBirth,
                CountryId = person.CountryId,
                CountryName = person.Country?.CountryName,
                Gender = Gender.Get(person.Gender),

                JobTitle = person.JobTitle,
                ContactNumber1 = person.ContactNumber1,
                ContactNumber2 = person.ContactNumber2,
                CompanyId = person.CompanyId,
                CompanyName = person.Company?.CompanyName,

                TagId = person.TagId,
                Tag = person.Tag?.ToContactTagResponse(),
                ContactGroups = person.ContactGroups?.Select(group => group.ToContactGroupResponse()).ToList(),
                ProfileBlobUrl = person.ProfileBlobUrl,
            };
        }
        public static PersonUpdateRequest ToPersonUpdateRequest(this PersonResponse personResponse)
        {
            return new PersonUpdateRequest()
            {
                PersonId = personResponse.PersonId,
                PersonName = personResponse.PersonName,
                Email = personResponse.Email,
                Address = personResponse.Address,
                DateOfBirth = personResponse.DateOfBirth,
                CountryId = personResponse.CountryId,
                Gender = personResponse.Gender,

                JobTitle = personResponse.JobTitle,
                ContactNumber1 = personResponse.ContactNumber1,
                ContactNumber2 = personResponse.ContactNumber2,
                CompanyId = personResponse.CompanyId,

                TagId = personResponse?.TagId,
                Tag = personResponse?.Tag,
                ContactGroups = personResponse?.ContactGroups?.Select(group => group.GroupId).ToList(),
                ProfileBlobUrl = personResponse?.ProfileBlobUrl
                // ContactLogs handled in ContactLogsService
            };
        }
    }
}
