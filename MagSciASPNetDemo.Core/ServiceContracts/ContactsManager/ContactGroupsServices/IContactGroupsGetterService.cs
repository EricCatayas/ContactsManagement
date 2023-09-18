using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.DTO.ContactsManager.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.ContactGroupsServices
{
    /// <summary>
    /// Defines a service for retrieving contact group data from the system.
    /// </summary>
    public interface IContactGroupsGetterService
    {   
        /// <summary>
        /// Retrieves list of person from the contact group.
        /// </summary>
        /// <returns>A list of persons from the corresponding contact group with the UserID</returns>
        Task<List<PersonResponse>> GetAllContactGroupPersons(int contactGroupId);
        /// <summary>
        /// Retrieves list of contact group from the system.
        /// </summary>
        /// <returns>A list of contact group object with the corresponding UserID</returns>
        Task<List<ContactGroupResponse>> GetAllContactGroups();
        /// <summary>
        /// Retrieves contact group from the system.
        /// </summary>
        /// <param name="contactGroupId">The request id of the contact group to be retrived.</param>
        /// <returns>The contact group object with the corresponding ID and UserID</returns>
        Task<ContactGroupResponse?> GetContactGroupById(int contactGroupId);
    }
}
