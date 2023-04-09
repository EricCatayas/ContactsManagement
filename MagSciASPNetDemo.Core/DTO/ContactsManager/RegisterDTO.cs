using ContactsManagement.Core.Enums.ContactsManager;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.DTO.ContactsManager
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name can't be blank")]
        public string? PersonName { get; set; }    
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Remote(action: "IsEmailAlreadyRegistered", controller:"Account")]
        public string? Email { get; set; }    
        [Required(ErrorMessage = "Phone number can't be blank")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must only contain numerics")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }    
        [Required(ErrorMessage = "Password can't be blank")]
        [MinLength(6, ErrorMessage = "Minimum length for password is 6 characters")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Confirm Password can't be blank")]
        [Compare("Password", ErrorMessage = "Invalid: Password does not match")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
        public UserRoleOptions userRole { get; set; } = UserRoleOptions.User;

    }
}
