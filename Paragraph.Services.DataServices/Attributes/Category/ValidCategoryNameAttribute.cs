using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace Paragraph.Services.DataServices.Attributes
{
    public class ValidCategoryNameAttribute : ValidationAttribute
    {
      
        protected override ValidationResult IsValid(object articleName, ValidationContext validationContext)
        {
            var service = (ICategoryService)validationContext.GetService(typeof(ICategoryService));

            bool doesCategoryExist = service.DoesCategoryNameExist(articleName.ToString());

            if (!doesCategoryExist)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"Category with {articleName.ToString()} already exists!");
            }
        }


    }
}
