using ContactsManagement.Core.DTO.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices
{
    public interface IPersonsGroupIdFilteredGetterService
    {
        Task<List<PersonResponse>> GetFilteredPersons(int? groupId, string? searchProperty, string? searchString);
    }
}
