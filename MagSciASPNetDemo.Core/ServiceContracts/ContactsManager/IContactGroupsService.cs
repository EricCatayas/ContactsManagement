using ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager
{
    public interface IContactGroupsService : IContactGroupsAdderService, IContactGroupsGetterService, IContactGroupsDeleterService
    {

    }
}
