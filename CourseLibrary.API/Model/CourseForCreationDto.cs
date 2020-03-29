using CourseLibrary.API.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Model
{
    [CourseTitleMustBeDifferentFromDescriptionAttributes(ErrorMessage = "The title must defferent from description. ")]
    public class CourseForCreationDto : CourseForManipulationDto//:IValidatableObject
    {
       
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if(Title == Description )
        //    {
        //        yield return new ValidationResult("The provider description shold be deffranet from the title.", new[] { "CourseForCreationDto" });

        //    }
        //}
    }
}

