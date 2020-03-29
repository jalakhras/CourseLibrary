using CourseLibrary.API.Model;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.ValidationAttributes
{
    public class CourseTitleMustBeDifferentFromDescriptionAttributes : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var course = (CourseForManipulationDto)validationContext.ObjectInstance;


            if (course.Title == course.Description)
            {
                return new ValidationResult(ErrorMessage, new[] {nameof(CourseForManipulationDto) });
            }

            return ValidationResult.Success;
        }
    }
}
