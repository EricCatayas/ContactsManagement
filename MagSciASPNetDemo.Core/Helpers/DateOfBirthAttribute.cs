using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Helpers
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DateOfBirthAttribute : ValidationAttribute
    {

        public int MinAge { get; set; } = 3;
        public int MaxAge { get; set; } = 100;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;


            var val = (DateTime)value;

            if (val.AddYears(MinAge) > DateTime.Now)
                return new ValidationResult($"Age must not be less than {MinAge} years old");

            return val.AddYears(MaxAge) > DateTime.Now ? ValidationResult.Success :
                new ValidationResult("Date of Birth is invalid");
        }
    }
}
