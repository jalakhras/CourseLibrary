using CourseLibrary.API.ValidationAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Model
{
    [CourseTitleMustBeDifferentFromDescriptionAttributes(ErrorMessage ="The title must defferent from description. ")]
    public class CourseForCreationDto //:IValidatableObject
    {
        [Required(ErrorMessage ="You should fill out title.")]
        [MaxLength(100,ErrorMessage ="The title shouldn't have more than 1500 characters")]
        public string Title { get; set; }
        [MaxLength(500,ErrorMessage ="The description shouldn't have more than 1000 characters .")]
        public string Description { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if(Title == Description )
        //    {
        //        yield return new ValidationResult("The provider description shold be deffranet from the title.", new[] { "CourseForCreationDto" });
                
        //    }
        //}
    }
}

