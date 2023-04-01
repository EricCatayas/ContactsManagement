using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.DTO.CompaniesManagement
{
    public class CompanyResponse
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string? CompanyDescription { get; set; }
        public string? Industry { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? ContactEmail { get; set; }
        //public string? CompanyProfileBlobUrl { get; set; } TODO
        public string? WebUrl { get; set; }
        public string? ContactNumber1 { get; set; }
        public string? ContactNumber2 { get; set; }
        public List<PersonResponse>? Employees { get; set; }
    }
    public static class CompanyResponseExtentions
    {
        public static CompanyResponse ToCompanyResponse(this Company company)
        {
            return new CompanyResponse()
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                CompanyDescription = company.CompanyDescription,
                Industry = company.Industry,
                Address1 = company.Address1,
                Address2 = company.Address2,
                ContactEmail = company.ContactEmail,
                WebUrl = company.WebUrl,
                ContactNumber1 = company.ContactNumber1,
                ContactNumber2 = company.ContactNumber2,
                Employees = company.Employees?.Select(person => person.ToPersonResponse()).ToList()
            };
        }
        public static CompanyUpdateRequest ToCompanyUpdateRequest(this CompanyResponse companyResponse)
        {
            return new CompanyUpdateRequest()
            {
                CompanyId = companyResponse.CompanyId,
                CompanyName = companyResponse.CompanyName,
                CompanyDescription = companyResponse.CompanyDescription,
                Industry = companyResponse.Industry,
                Address1 = companyResponse.Address1,
                Address2 = companyResponse.Address2,
                ContactEmail = companyResponse.ContactEmail,
                WebUrl = companyResponse.WebUrl,
                ContactNumber1 = companyResponse.ContactNumber1,
                ContactNumber2 = companyResponse.ContactNumber2,
                Employees = companyResponse.Employees?.Select(person => person.PersonId).ToList(),
            };
        }
    }
}
