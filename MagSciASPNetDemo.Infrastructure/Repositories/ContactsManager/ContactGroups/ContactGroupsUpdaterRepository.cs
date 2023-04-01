using ContactsManagement.Core.Domain.Entities.ContactsManager;
using ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager;
using ContactsManagement.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Infrastructure.Repositories.ContactsManager.ContactGroups
{
    public class ContactGroupsUpdaterRepository : IContactGroupsUpdaterRepository
    {
        private readonly ApplicationDbContext _db;

        public ContactGroupsUpdaterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<ContactGroup> UpdateContactGroup(ContactGroup contactGroup)
        {
            ContactGroup? contactGroupToUpdate = await _db.ContactGroups.Include(temp => temp.Persons).FirstOrDefaultAsync(temp => temp.GroupId == contactGroup.GroupId);

            if (contactGroupToUpdate == null)
                return contactGroup;

            contactGroupToUpdate.Persons = contactGroup.Persons;
            contactGroupToUpdate.GroupName = contactGroup.GroupName;
            contactGroupToUpdate.Description = contactGroup.Description;

            await _db.SaveChangesAsync();

            return contactGroupToUpdate;
        }
    }
}
