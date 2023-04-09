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

namespace ContactsManagement.Core.Services.ContactsManager.Persons
{
    public class PersonsAdderService : IPersonsAdderService
    {
        private readonly IPersonsAdderRepository _personsRepository;
        private readonly IContactGroupsGetterRepository _contactGroupsRepository;
        public PersonsAdderService(IPersonsAdderRepository personsRepository, IContactGroupsGetterRepository contactGroupsRepository)
        {
            _personsRepository = personsRepository;
            _contactGroupsRepository = contactGroupsRepository;
        }
        public async Task<PersonResponse?> AddPerson(PersonAddRequest? personAddRequest, Guid userId)
        {
            if (personAddRequest == null)
                throw new ArgumentNullException(nameof(personAddRequest));
            try
            {
                ValidationHelper.ModelValidation(personAddRequest);
                Person newPerson = personAddRequest.ToPerson();
                newPerson.UserId = userId;
                //ContactGroups
                if (personAddRequest.ContactGroups != null)
                    newPerson.ContactGroups = await _contactGroupsRepository.GetContactGroupsById(personAddRequest.ContactGroups);

                newPerson = await _personsRepository.AddPerson(newPerson);

                return newPerson.ToPersonResponse();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
