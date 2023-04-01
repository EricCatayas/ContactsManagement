using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using OfficeOpenXml;
using SerilogTimings;
using Serilog;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Helpers;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.Domain.Entities.ContactsManager;

namespace ContactsManagement.Core.Services.ContactsManager
{
    public class PersonsAdderService : IPersonsAdderService
    {
        private readonly IPersonsAdderRepository _personsRepository;
        private readonly IDiagnosticContext _diagnosticContext;
        private readonly IContactGroupsGetterRepository _contactGroupsRepository;
        public PersonsAdderService(IDiagnosticContext diagnostics, IPersonsAdderRepository personsRepository, IContactGroupsGetterRepository contactGroupsRepository)
        {
            _diagnosticContext = diagnostics;
            _personsRepository = personsRepository;
            _contactGroupsRepository = contactGroupsRepository;
        }
        public async Task<PersonResponse?> AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null)
                throw new ArgumentNullException(nameof(personAddRequest));
            try
            {
                ValidationHelper.ModelValidation(personAddRequest);
                Person newPerson = personAddRequest.ToPerson();
                newPerson = await _personsRepository.AddPerson(newPerson);

                //ContactGroups
                if (personAddRequest.ContactGroups != null)
                    newPerson.ContactGroups = await _contactGroupsRepository.GetContactGroups(personAddRequest.ContactGroups);

                _diagnosticContext.Set("Person Added:", $"{newPerson.Name}, {newPerson.Address}");
                return newPerson.ToPersonResponse();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
