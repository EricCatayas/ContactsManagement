﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.RepositoryContracts.ContactsManager
{
    public interface IContactTagsDeleterRepository
    {
        Task<bool> DeleteContactTagById(int contactTagId, Guid userId);
    }
}
