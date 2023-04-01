using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Infrastructure.DbContexts;
using ContactsManagement.Infrastructure.Repositories.ContactsManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactGroups
{
    public class ContactGroupsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ContactGroupsRepository> _logger;

        public ContactGroupsRepository(ApplicationDbContext db, ILogger<ContactGroupsRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<ContactGroup> AddContactGroup(ContactGroup contactGroup, List<Guid>? persons)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(ContactGroupsRepository), nameof(AddContactGroup));

            contactGroup.GroupId = _db.ContactGroups.Count() + 1;
            _db.ContactGroups.Add(contactGroup);
            _db.SaveChanges();

            if (persons != null)
            {
                List<Person>? personsList = await _db.Persons.Where(person => persons.Contains(person.Id)).ToListAsync();

                if (personsList != null)
                {
                    contactGroup.Persons = personsList;
                    foreach (Person person in personsList)
                    {
                        person.ContactGroups ??= new List<ContactGroup>();
                        person.ContactGroups.Add(contactGroup);
                    }
                    await _db.SaveChangesAsync();
                }

            }
            return contactGroup;
        }

        public async Task<bool> AddContactGroupsToPerson(Person person, List<int>? contactGroups)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(ContactGroupsRepository), nameof(AddContactGroupsToPerson));
            List<ContactGroup>? contactGroupsToAdd = await GetContactGroups(contactGroups);

            if (contactGroups == null)
                return false;
            try
            {
                foreach (ContactGroup group in contactGroupsToAdd)
                {
                    person.ContactGroups ??= new List<ContactGroup>();
                    person.ContactGroups.Add(group);
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding contact groups to person; undoing transaction", ex);
            }
        }

        public async Task<bool> DeleteContactGroup(ContactGroup contactGroup)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(ContactGroupsRepository), nameof(DeleteContactGroup));

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                if (contactGroup.Persons != null)
                {
                    foreach (Person person in contactGroup.Persons)
                    {
                        person.ContactGroups = null;
                        _db.Entry(person).State = EntityState.Modified;
                    }
                }

                _db.ContactGroups.RemoveRange(contactGroup);
                int rowsDeleted = await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return rowsDeleted > 0;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error removing ContactGroup and Persons; undoing transaction", ex);
            }
        }

        public async Task<List<Person>?> GetAllPersons(ContactGroup contactGroup)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(ContactGroupsRepository), nameof(GetAllPersons));

            var persons = await _db.Persons.Include(p => p.ContactGroups).ToListAsync();
            return persons.Where(p => p.ContactGroups.Contains(contactGroup)).ToList();
        }

        public async Task<ContactGroup?> GetContactGroupById(int contactGroupId)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(ContactGroupsRepository), nameof(GetContactGroupById));

            return await _db.ContactGroups.Include(p => p.Persons).FirstOrDefaultAsync(c => c.GroupId == contactGroupId);
        }

        public async Task<List<ContactGroup>?> GetContactGroups(List<int>? contactGroupIds)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(ContactGroupsRepository), nameof(GetContactGroups));

            return contactGroupIds != null ?
                await _db.ContactGroups.Include(p => p.Persons).Where(c => contactGroupIds.Contains(c.GroupId)).ToListAsync() :
                await _db.ContactGroups.Include(p => p.Persons).ToListAsync();
        }

        public async Task<bool> RemoveContactGroupPersons(int contactGroupId, List<Person> persons)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(ContactGroupsRepository), nameof(RemoveContactGroupPersons));

            try
            {
                foreach (var person in persons)
                {
                    var contactGroup = person.ContactGroups.FirstOrDefault(cg => cg.GroupId == contactGroupId);
                    if (contactGroup != null)
                    {
                        person.ContactGroups.Remove(contactGroup);
                        _db.Entry(person).State = EntityState.Modified;
                    }
                }

                int changes = await _db.SaveChangesAsync();
                return changes > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing contact group from persons; undoing transaction", ex);
            }
        }

        public async Task<bool> RemoveContactGroupsFromPerson(Person person, List<ContactGroup> contactGroups)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(ContactGroupsRepository), nameof(RemoveContactGroupsFromPerson));

            try
            {
                if (person != null)
                {
                    foreach (var contactGroup in contactGroups)
                    {
                        if (contactGroup.Persons.Contains(person))
                        {
                            contactGroup.Persons.Remove(person);
                            _db.Entry(contactGroup).State = EntityState.Modified;
                        }
                    }

                    int changes = await _db.SaveChangesAsync();
                    return changes > 0;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing person from contact groups; undoing transaction", ex);
            }
        }

        public async Task<ContactGroup> UpdateContactGroup(ContactGroup contactGroup)
        {
            _logger.LogDebug("{RepositoryName}.{MethodName}", nameof(ContactGroupsRepository), nameof(UpdateContactGroup));
            ContactGroup? contactGroupToUpdate = await GetContactGroupById(contactGroup.GroupId);

            if (contactGroupToUpdate == null)
                return contactGroup;

            contactGroupToUpdate.Persons = contactGroup.Persons;
            contactGroupToUpdate.GroupName = contactGroup.GroupName;
            contactGroupToUpdate.Description = contactGroup.Description;

            await _db.SaveChangesAsync();

            return contactGroupToUpdate;
        }

        public Task<bool> UpdateContactGroupsFromPerson(Person person, List<ContactGroup>? contactGroups)
        {
            /* Hallelujah! Yes, if you add a Person object to the Company.Persons collection and call SaveChanges() on the database context,
               the relationship between the Person and Company entities will be saved in the database ~ChatGPT.*/

            // previousContactGroupsData
            // previousPersons
            // personsToAdd
            // if(previousPersons != null)
            // {
            //    if (personsToAdd != null)
            //    {
            //       List<Person>? removedPersons = previousPersons.Where(PersonA => !PersonsToAdd.Any(PersonB => PersonB.Id == PersonA.Id)).ToList();
            //        if (removedPersons != null)
            //            await this.RemoveContactGroupPersons(previousContactGroupData, removedPersons);
            //    }
            //    else // i.e. all employess are removed
            //    {
            //        await this.RemoveContactGroupPersons(previousContactGroupData, previousPersons);
            //    }
            // }
            // if (PersonsToAdd != null)
            // {
            //    ContactGroup = await _companiesRepository.AddContactGroupWithPersons(ContactGroup, PersonsToAdd);
            // }

            // ContactGroup = await _companiesRepository.UpdateContactGroup(ContactGroup);
            throw new NotImplementedException();
        }
    }
}
