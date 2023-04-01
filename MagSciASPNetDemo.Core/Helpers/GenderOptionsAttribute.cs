using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Helpers
{
    /// <summary>
    /// Tried casting a string object bind to Gender, didn't work
    /// </summary>
    public class GenderOptionsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is string)
            {
                try
                {
                    return new ValidationResult("Gotta restart");
                }
                catch (Exception)
                {
                    return new ValidationResult($"Invalid value for {validationContext.DisplayName}");
                }
            }

            return new ValidationResult($"{validationContext.DisplayName} must be of type string or Gender");
        }
    }
}
