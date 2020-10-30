using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebInvoicer.Core.Attributes
{
    public class ValidNip : ValidationAttribute
    {
        private readonly string errorMessage = "Specified string is not a valid NIP number";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var nip = value as string;

            if (nip.Length != 10 || !nip.All(x => Char.IsDigit(x)))
            {
                return new ValidationResult(errorMessage);
            }

            var weights = new[] { 6, 5, 7, 2, 3, 4, 5, 6, 7, 0 };
            var controlSum = nip.Zip(weights, (digit, weight) => (digit - '0') * weight).Sum();
            var isValid = (controlSum % 11) == (nip[9] - '0');

            return isValid ? ValidationResult.Success : new ValidationResult(errorMessage);
        }
    }
}
