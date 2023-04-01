using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManagement.Core.Helpers
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MinimumDateRangeAttribute : ValidationAttribute
    {

        public int Year { get; set; } = 2000;
        //new DateTime(2000, 1, 1);
        public int Month { get; set; } = 1;
        public int Day { get; set; } = 1;
        private DateTime? FromDate;
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            FromDate = new DateTime(Year, Month, Day);
            var val = (DateTime)value;

            if (val.Date < FromDate)
                return new ValidationResult(ErrorMessage ?? $"Date should not be older than {FromDate.Value.ToString("MMM dd yyyy")}");

            return ValidationResult.Success;
        }
    }
}
