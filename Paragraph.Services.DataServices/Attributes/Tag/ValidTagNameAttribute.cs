using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Paragraph.Services.DataServices.Attributes
{
    public class ValidTagNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object tagName, ValidationContext validationContext)
        {
            var service = (ITagService)validationContext.GetService(typeof(ITagService));

            bool isNameTaken = service.DoesТаgNameExist(tagName.ToString());

            if (!isNameTaken)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"Tag with name {tagName.ToString()} already exists");
            }
        }
    }
}
