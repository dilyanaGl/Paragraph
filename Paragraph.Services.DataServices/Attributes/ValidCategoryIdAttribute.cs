using System;
using System.ComponentModel.DataAnnotations;

namespace Paragraph.Services.DataServices.Attributes
{
    public class ValidCategoryIdAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object categoryId, ValidationContext validationContext)
        {
            var service = (ICategoryService)validationContext.GetService(typeof(ICategoryService));

            bool success = service.IsCategoryVald((int)categoryId);

            if(success)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Invalid category Id!");
            }
        }
    }
}