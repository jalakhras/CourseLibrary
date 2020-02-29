using AutoMapper;
using CourseLibrary.API.Model;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;
        public AuthorController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ?? throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        //[HttpGet()]
        //[HttpHead]
        //public ActionResult<IEnumerable<AuthorDto>> GetAuthors()

        //{

        //    var autors = _courseLibraryRepository.GetAuthors();
        //    return new JsonResult(_mapper.Map<IEnumerable<AuthorDto>>(autors));
        //}
        [HttpGet()]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors(string mainCategory , string searchQuery)

        {

            var autors = _courseLibraryRepository.GetAuthors(mainCategory, searchQuery);
            return new JsonResult(_mapper.Map<IEnumerable<AuthorDto>>(autors));
        }

        [HttpGet("{authorId:guid}")]
        public IActionResult GetAuthors(Guid authorId)
        {

            var autors = _courseLibraryRepository.GetAuthor(authorId);
            if (autors == null) return NotFound();
            return new JsonResult(_mapper.Map<AuthorDto>(autors));
        }
    }
}
