using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Helpers
{
    public static class ValidationHelper
    {
        public static void ModelValidation(object obj)
        {
            ValidationContext personContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool IsValid = Validator.TryValidateObject(obj, personContext, validationResults, true);
            if (!IsValid)
            {
                string ErrorMessages = string.Join(" ", validationResults.Select(result => result.ErrorMessage));
                throw new ArgumentException(ErrorMessages);
            }
        }
    }
}
