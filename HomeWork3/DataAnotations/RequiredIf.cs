using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeWork3.DataAnotations.Attributes
{
    public class RequiredIf : ValidationAttribute, IClientValidatable
    {
        public string OtherProperty { get; set; }
        public object OtherPropertyValue { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var targetProperty = validationContext.ObjectType.GetProperty(OtherProperty);
            var targetPropertyValue = targetProperty.GetValue(validationContext.ObjectInstance, null);

            if (targetPropertyValue != null && targetPropertyValue.Equals(OtherPropertyValue))
            {
                if (value != null)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    var errorMessage = String.IsNullOrEmpty(ErrorMessage) ?
                        String.Format("Cused error: {0}, {1}, {2}", validationContext.DisplayName, OtherProperty, OtherPropertyValue.ToString()) :
                        ErrorMessage;

                    return new ValidationResult(errorMessage);
                }
            }
            else
            {
                return ValidationResult.Success;
            }
        }

        public IEnumerable<ModelClientValidationRule>
       GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "requiredif"
            };
        }
    }
}