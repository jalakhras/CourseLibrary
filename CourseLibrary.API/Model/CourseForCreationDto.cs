using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Model
{
    public class CourseForCreationDto :IValidatableObject
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Title == Description )
            {
                yield return new ValidationResult("The provider description shold be deffranet from the title.", new[] { "CourseForCreationDto" });
                
            }
        }
    }
}

