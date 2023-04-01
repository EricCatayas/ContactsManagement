using ContactsManagement.Core.Domain.Entities.ContactsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.DTO.ContactsManager
{
    public class CountryAddRequest
    {
        public string? CountryName { get; set; }
        public Country toCountry()
        {
            return new Country()
            {
                CountryName = CountryName,
                CountryId = Guid.NewGuid(),
            };
        }
    }
}
