using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        public AuthorController(ICourseLibraryRepository courseLibraryRepository)
        {
            _courseLibraryRepository = courseLibraryRepository ?? throw new ArgumentNullException(nameof(courseLibraryRepository));
        }
        [HttpGet()]
        public IActionResult GetAuthors()
        {
            var autors = _courseLibraryRepository.GetAuthors();
            return new JsonResult(autors);
        }

        [HttpGet("{authorId:guid}")]
        public IActionResult GetAuthors(Guid authorId)
        {

            var autors = _courseLibraryRepository.GetAuthor(authorId);
            if (autors == null) return NotFound();
            return new JsonResult(autors);
        }
    }
}
