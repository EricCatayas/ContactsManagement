using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using Serilog;
using Serilog.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactsManagement.Core.DTO.CompaniesManagement;
using ContactsManagement.Core.ServiceContracts.CompaniesManagement;
using ContactsManagement.Core.Domain.RepositoryContracts.CompaniesManagement;

namespace ContactsManagement.Core.Services.CompaniesManagement
{
    public class CompaniesService : ICompaniesService
    {
        private readonly ICompaniesRepository _companiesRepository;
        private readonly IPersonsRepository _personsRepository;
        private readonly IDiagnosticContext _diagnosticContext;

        public CompaniesService(IDiagnosticContext diagnosticContext, ICompaniesRepository companiesRepository, IPersonsRepository personsRepository)
        {
            _companiesRepository = companiesRepository;
            _personsRepository = personsRepository;
            _diagnosticContext = diagnosticContext;
        }
        public async Task<CompanyResponse> AddCompany(CompanyAddRequest companyAddRequest)
        {
            Company company = companyAddRequest.ToCompany();

            if (companyAddRequest.Employees != null)
            {
                List<Person>? persons = await _personsRepository.GetPersonsById(companyAddRequest.Employees);
                company = await _companiesRepository.AddCompanyWithEmployees(company, persons);
            }
            else
            {
                company = await _companiesRepository.AddCompany(company);
            }
            _diagnosticContext.Set("CompanyAdded", company.CompanyName);
            return company.ToCompanyResponse();
        }

        public async Task<bool> DeleteCompany(int companyID)
        {
            Company companyToDelete = await _companiesRepository.GetCompanyById(companyID);
            return await _companiesRepository.DeleteCompany(companyToDelete);
        }

        public async Task<List<CompanyResponse>?> GetAllCompanies()
        {
            List<Company>? companies = await _companiesRepository.GetAllCompanies();
            return companies?.Select(company => company.ToCompanyResponse()).ToList().OrderBy(company => company.CompanyName).ToList();
        }

        public async Task<CompanyResponse?> GetCompanyById(int companyID)
        {
            Company? company = await _companiesRepository.GetCompanyById(companyID);
            return company?.ToCompanyResponse();
        }

        public async Task<List<PersonResponse>?> GetCompanyEmployees(int companyID)
        {
            //Order by Descending
            List<Person>? persons = await _companiesRepository.GetCompanyEmployees(companyID);
            return persons?.OrderByDescending(person => person.Name).ToList().Select(person => person.ToPersonResponse()).ToList();
        }

        public async Task<CompanyResponse> UpdateCompany(CompanyUpdateRequest companyUpdateRequest)
        {
            // Hallelujah!  Yes, if you add a Person object to the Company.Persons collection and call SaveChanges() on the database context, the relationship between the Person and Company entities will be saved in the database.

            /*Company? previousCompanyData = await _companiesRepository.GetCompanyById(companyUpdateRequest.CompanyId);
            List<Person>? previousEmployees = previousCompanyData.Employees?.ToList();            
            List<Person>? employeesToAdd = 
            if(previousEmployees != null)
            {
                if(employeesToAdd != null) 
                {
                    List<Person>? removedEmployees = previousEmployees.Where(employeeA => !employeesToAdd.Any(employeeB => employeeB.Id == employeeA.Id)).ToList();
                    if (removedEmployees != null)
                        await _companiesRepository.RemoveCompanyEmployees(previousCompanyData, removedEmployees);
                }
                else // i.e. all employess are removed
                {
                    await _companiesRepository.RemoveCompanyEmployees(previousCompanyData, previousEmployees);
                }
            }
            if(employeesToAdd != null)
            {
                company = await _companiesRepository.AddCompanyWithEmployees(company, employeesToAdd);
            }*/
            Company company = companyUpdateRequest.ToCompany();
            company.Employees = await _personsRepository.GetPersonsById(companyUpdateRequest.Employees);
            company = await _companiesRepository.UpdateCompany(company);

            return company.ToCompanyResponse();
        }
    }
}
