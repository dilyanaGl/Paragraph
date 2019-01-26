using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Paragraph.Services.DataServices.Attributes
{
    public class ValidTagIdAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object tagId, ValidationContext validationContext)
        {
            var service = (ITagService)validationContext.GetService(typeof(ITagService));

            bool success = service.IsTagValid((int)tagId);

            if (success)
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
