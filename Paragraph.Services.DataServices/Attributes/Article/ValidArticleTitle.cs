using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Paragraph.Services.DataServices.Attributes.Article
{
    class ValidArticleTitle : ValidationAttribute
    {
        protected override ValidationResult IsValid(object categoryName, ValidationContext validationContext)
        {
            var service = (IArticleService)validationContext.GetService(typeof(IArticleService));

            bool doesArticleNameExist = service.DoesArticleNameExist(categoryName.ToString());

            if (!doesArticleNameExist)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"Article with title {categoryName.ToString()} already exists!");
            }
        }
    }
}
