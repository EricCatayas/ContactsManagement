using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Domain.IdentityEntities
{
    /// <summary>
    /// Nuget: AspNetCore.Identity, AspNetCore.Identity.EF
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }
    }
}
