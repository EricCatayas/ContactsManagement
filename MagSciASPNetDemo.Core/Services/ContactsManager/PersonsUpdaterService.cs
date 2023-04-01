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
using Serilog;
using SerilogTimings;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Helpers;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;

namespace ContactsManagement.Core.Services.ContactsManager
{
    public class PersonsUpdaterService : IPersonsUpdaterService
    {
        // R: Logger in Repository
        private readonly IPersonsUpdaterRepository _personsUpdaterRepository;
        private readonly IDiagnosticContext _diagnosticContext;
        private readonly IContactGroupsGetterRepository _contactGroupsRepository;

        public PersonsUpdaterService(IDiagnosticContext diagnostics, IPersonsUpdaterRepository personsRepository, IContactGroupsGetterRepository contactGroupsRepository)
        {
            _diagnosticContext = diagnostics;
            _personsUpdaterRepository = personsRepository;
            _contactGroupsRepository = contactGroupsRepository;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest) //TODO Azure Image Uploading
        {
            if (personUpdateRequest == null)
                throw new ArgumentNullException("Person must not be null");
            try
            {

                ValidationHelper.ModelValidation(personUpdateRequest);
                
                /* Note: Company CRUD for Person is handled in CompaniesService        :: many-to-1
                         ContactLog CRUD for Person is handled in ContactLogsService   :: 1-to-1 
                         ContactTag Update/Removal is handled in ContactTagsUpdaterService      */

                Person updatedPerson = personUpdateRequest.ToPerson();
                if(personUpdateRequest.ContactGroups != null)
                {
                    updatedPerson.ContactGroups = await _contactGroupsRepository.GetContactGroups(personUpdateRequest.ContactGroups);
                }

                updatedPerson = await _personsUpdaterRepository.UpdatePerson(updatedPerson);

                return updatedPerson.ToPersonResponse();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            //_personsDbContext.sp_UpdatePerson(personToUpdate);  <-- Borken Stored Procedure            
        }
    }
}
