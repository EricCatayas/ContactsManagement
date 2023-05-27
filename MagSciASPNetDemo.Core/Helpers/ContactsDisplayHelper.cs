using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Helpers
{
    public static class ContactsDisplayHelper
    {
        public static Dictionary<string, string> GetDisplayProperties(ContactsDisplayType displayType)
        {
            switch(displayType)
            {
                case ContactsDisplayType.Profile:
                    return new Dictionary<string, string>()
                    {
                        { "Name", nameof(PersonResponse.PersonName) },
                        { "Email", nameof(PersonResponse.Email) },
                        { "Address", nameof(PersonResponse.Address) },
                        { "Birthday", nameof(PersonResponse.DateOfBirth) },
                        { "Tag", nameof(ContactTagDTO.TagName) },
                    };
                case ContactsDisplayType.Contact:
                    return new Dictionary<string, string>()
                    {
                        { "Name", nameof(PersonResponse.PersonName) },
                        { "Email", nameof(PersonResponse.Email) },
                        { "Number", string.Empty },
                        { "Tag", nameof(ContactTagDTO.TagName) },
                    };
                case ContactsDisplayType.Employment:
                    return new Dictionary<string, string>()
                    {
                        { "Name", nameof(PersonResponse.PersonName) },
                        { "Job Title", nameof(PersonResponse.JobTitle) },
                        { "Company Name", nameof(PersonResponse.CompanyName) },
                        { "Tag", nameof(ContactTagDTO.TagName) },
                    };
                default:
                    return new Dictionary<string, string>()
                    {
                        { "Name", nameof(PersonResponse.PersonName) },
                        { "Job Title", nameof(PersonResponse.JobTitle) },
                        { "Company Name", nameof(PersonResponse.CompanyName) },
                        { "Tag", nameof(ContactTagDTO.TagName) },
                    };
            }
        }
        public static ContactsDisplayType GetDisplayType(string? contactDisplayType)
        {
            try
            {
                return (ContactsDisplayType)Enum.Parse(typeof(ContactsDisplayType), contactDisplayType, true);
            }
            catch
            {
                return ContactsDisplayType.Profile;
            }
        }
    }
}
