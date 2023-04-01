using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Helpers
{
    public class StartDateBeforeEndDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startDate = (DateTime?)validationContext.ObjectType.GetProperty("StartDate")?.GetValue(validationContext.ObjectInstance, null);
            var endDate = (DateTime?)validationContext.ObjectType.GetProperty("EndDate")?.GetValue(validationContext.ObjectInstance, null);

            if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
            {
                return new ValidationResult("The start date must come before the end date.");
            }

            return ValidationResult.Success;
        }
    }
}
