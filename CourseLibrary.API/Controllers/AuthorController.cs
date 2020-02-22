using CourseLibrary.API.Helper;
using CourseLibrary.API.Model;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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
            var autirsDto = autors.Select(item => new AuthorDto
            {
                Id = item.Id,
                Name = $"{item.FirstName}{item.LastName}",
                Age = item.DateOfBirth.GetCurrantAge()

            }).ToList();
            return new JsonResult(autirsDto);
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
