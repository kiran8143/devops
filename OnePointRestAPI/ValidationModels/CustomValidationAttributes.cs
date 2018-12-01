using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnePointRestAPI.ValidationModels
{
    public class MCNCheck : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty("AccountNumber");

            if (field != null)
            {
                dynamic extensionValue = field.GetValue(validationContext.ObjectInstance, null);
                if (extensionValue != null)
                {
                    return extensionValue.StartsWith("MCN") ? ValidationResult.Success : new ValidationResult("Please Verify the MCN format MCNXXXXXXXX ", new[] { validationContext.MemberName });
                }
            }
            return ValidationResult.Success;
        }

    }
    public class MFRefCheck : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty("MFRef");

            if (field != null)
            {
                dynamic extensionValue = field.GetValue(validationContext.ObjectInstance, null);
                if (extensionValue != null)
                {
                    return extensionValue.StartsWith("MF") ? ValidationResult.Success : new ValidationResult("Please Verify the MFRef format MFXXXXXX18 ", new[] { validationContext.MemberName });
                }
            }   
            return ValidationResult.Success;
        }
        
    }
    public class PTRIdCheck : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty("PTRId");

            if (field != null)
            {
                dynamic extensionValue = field.GetValue(validationContext.ObjectInstance, null);
                
                    return ((extensionValue>0)&& (extensionValue<int.MaxValue)) ? ValidationResult.Success : new ValidationResult("Invalid PTRId  ", new[] { validationContext.MemberName });
                
            }
            return ValidationResult.Success;
        }

    }
    
}
