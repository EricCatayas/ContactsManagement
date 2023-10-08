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
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.Exceptions;

namespace ContactsManagement.Core.Services.ContactsManager.Persons
{
    public class PersonsAdderService : IPersonsAdderService
    {
        private readonly IPersonsAdderRepository _personsRepository;
        private readonly IContactGroupsGetterRepository _contactGroupsRepository;
        private readonly ISignedInUserService _signedInUserService;

        public PersonsAdderService(IPersonsAdderRepository personsRepository, IContactGroupsGetterRepository contactGroupsRepository, ISignedInUserService signedInUserService)
        {
            _personsRepository = personsRepository;
            _contactGroupsRepository = contactGroupsRepository;
            _signedInUserService = signedInUserService;
        }
        //TODO Code Refactor
        public async Task<PersonResponse?> AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null)
                throw new ArgumentNullException(nameof(personAddRequest));
            Guid? userId = _signedInUserService.GetSignedInUserId();
            if (userId == null)
                throw new AccessDeniedException();
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
