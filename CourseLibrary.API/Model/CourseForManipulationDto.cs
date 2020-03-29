using CourseLibrary.API.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Model
{
    [CourseTitleMustBeDifferentFromDescriptionAttributes(ErrorMessage = "The title must defferent from description. ")]
    public abstract class CourseForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out title.")]
        [MaxLength(100, ErrorMessage = "The title shouldn't have more than 1500 characters")]
        public string Title { get; set; }
        [MaxLength(500, ErrorMessage = "The description shouldn't have more than 1000 characters .")]
        public virtual string Description { get; set; }

    }
}
