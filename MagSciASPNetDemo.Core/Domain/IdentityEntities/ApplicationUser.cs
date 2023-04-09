using ContactsManagement.Core.Domain.Entities.ContactsManager;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company? company { get; set; } // navigation property for null checking
    }
}
