using Serilog;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Enums.ContactsManager;
using ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices;
using ContactsManagement.Core.Domain.Entities.ContactsManager;

namespace ContactsManagement.Core.Services.ContactsManager
{
    public class PersonsSorterService : IPersonsSorterService
    {
        public PersonsSorterService()
        {
        }
        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOptions)
        {
            List<PersonResponse> sortedList = allPersons;
            if (sortOptions.CompareTo(SortOrderOptions.ASC) == 0) // if ASC
            {
                switch (sortBy)
                {
                    case nameof(Person.Name):
                        sortedList = allPersons.OrderBy(person => person.PersonName).ToList();
                        break;
                    case nameof(Person.Address):
                        sortedList = allPersons.OrderBy(person => person.Address).ToList();
                        break;
                    case nameof(Person.Email):
                        sortedList = allPersons.OrderBy(person => person.Email).ToList();
                        break;
                    case nameof(ContactTag.TagName):
                        sortedList = allPersons.OrderBy(person => person.Tag?.TagName).ToList();
                        break;
                    case nameof(Person.JobTitle):
                        sortedList = allPersons.OrderBy(person => person.JobTitle).ToList();
                        break;
					case nameof(Person.Company.CompanyName):
						sortedList = allPersons.OrderBy(person => person.CompanyName).ToList();
						break;
				};
            }
            else
            {
                switch (sortBy)
                {
                    case nameof(Person.Name):
                        sortedList = allPersons.OrderByDescending(person => person.PersonName).ToList();
                        break;
                    case nameof(Person.Address):
                        sortedList = allPersons.OrderByDescending(person => person.Address).ToList();
                        break;
                    case nameof(Person.Email):
                        sortedList = allPersons.OrderByDescending(person => person.Email).ToList();
                        break;
                    case nameof(ContactTag.TagName):
                        sortedList = allPersons.OrderByDescending(person => person.DateOfBirth.Value.ToString("yyyy")).ToList();
                        break;
                    case nameof(Person.JobTitle):
                        sortedList = allPersons.OrderByDescending(person => person.JobTitle).ToList();
                        break;
					case nameof(Person.Company.CompanyName):
						sortedList = allPersons.OrderByDescending(person => person.CompanyName).ToList();
						break;
				};
            }
            return sortedList;
        }
    }
}
