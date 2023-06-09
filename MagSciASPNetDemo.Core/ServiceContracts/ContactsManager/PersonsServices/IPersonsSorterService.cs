﻿using ContactsManagement.Core.DTO.ContactsManager;
using ContactsManagement.Core.Enums.ContactsManager;

namespace ContactsManagement.Core.ServiceContracts.ContactsManager.PersonsServices
{
    /// <summary>
    /// Defines a service for sorting Person data from the system.
    /// </summary>
    public interface IPersonsSorterService
    {
        /// <summary>
        /// Returns the sorted list of Person in either ascending or descending order
        /// </summary>
        /// <param name="allPersons">the list of Person to be sorted</param>
        /// <param name="sortBy">the field property of Person to be sorted by</param>
        /// <param name="sortOptions">Ascending or Descending</param>
        /// <returns>returns the list argument in sorted order</returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOptions);
    }
}
