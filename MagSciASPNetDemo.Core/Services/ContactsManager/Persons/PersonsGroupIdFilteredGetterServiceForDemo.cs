using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using ContactsManagement.Core.Exceptions;
using ContactsManagement.Core.ServiceContracts.AccountManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Services.ContactsManager.Persons
{
    public class PersonsGroupIdFilteredGetterServiceForDemo : IPersonsGroupIdFilteredGetterService
    {
        private readonly IPersonsGetterRepository _personsGetterRepository;
        private readonly ISignedInUserService _signedInUserService;
        private readonly IDemoUserService _demoUserService;

        public PersonsGroupIdFilteredGetterServiceForDemo(IPersonsGetterRepository personsGetterRepository, ISignedInUserService signedInUserService, IDemoUserService demoUserService)
        {
            _personsGetterRepository = personsGetterRepository;
            _signedInUserService = signedInUserService;
            _demoUserService = demoUserService;
        }
        public async Task<List<PersonResponse>> GetFilteredPersons(int? groupId, string? searchProperty, string? searchString)
        {
            Guid userId = _signedInUserService.GetSignedInUserId() ?? Guid.Empty;
            if (userId == Guid.Empty)
                userId = _demoUserService.GetDemoUserId();

            List<Person> persons = new List<Person>() { };
            try
            {
                if (searchString != null)
                {
                    persons = searchProperty switch
                    {
                        nameof(PersonResponse.PersonName) =>
                            await _personsGetterRepository.GetFilteredPersons(temp => 
                            temp.ContactGroups.Any(group => group.GroupId == groupId) && temp.Name.Contains(searchString), userId),

                        nameof(PersonResponse.Email) =>
                            await _personsGetterRepository.GetFilteredPersons(temp =>
                            temp.ContactGroups.Any(group => group.GroupId == groupId) && temp.Email.Contains(searchString), userId),

                        nameof(PersonResponse.JobTitle) =>
                            await _personsGetterRepository.GetFilteredPersons(temp =>
                            temp.ContactGroups.Any(group => group.GroupId == groupId) && temp.JobTitle.Contains(searchString), userId),

                        nameof(PersonResponse.CompanyName) =>
                            await _personsGetterRepository.GetFilteredPersons(temp =>
                            temp.ContactGroups.Any(group => group.GroupId == groupId) && temp.Company.CompanyName.Contains(searchString), userId),

                        nameof(PersonResponse.Address) =>
                            await _personsGetterRepository.GetFilteredPersons(temp =>
                            temp.ContactGroups.Any(group => group.GroupId == groupId) && temp.Address.Contains(searchString), userId),

                        nameof(PersonResponse.CountryName) =>
                            await _personsGetterRepository.GetFilteredPersons(temp =>
                            temp.ContactGroups.Any(group => group.GroupId == groupId) && temp.Country.CountryName.Contains(searchString), userId),

                        nameof(ContactTagDTO.TagName) =>
                            await _personsGetterRepository.GetFilteredPersons(temp =>
                            temp.ContactGroups.Any(group => group.GroupId == groupId) && temp.Tag.TagName.Contains(searchString), userId),

                        _ => await _personsGetterRepository.GetFilteredPersons(temp => temp.ContactGroups.Any(group => group.GroupId == groupId), userId),
                    };
                }
                else
                {
                    persons = await _personsGetterRepository.GetFilteredPersons(temp => temp.ContactGroups.Any(group => group.GroupId == groupId), userId);
                }
            }
            catch (Exception ex)
            {
                return new List<PersonResponse>() { };
            }
            return persons.Select(person => person.ToPersonResponse()).ToList();
        }
    }
}
