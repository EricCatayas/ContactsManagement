using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.DTO.ContactsManager
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Input must be in proper email format")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
