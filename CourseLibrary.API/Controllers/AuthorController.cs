using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Model;
using CourseLibrary.API.ResourceParameter;
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
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery] AuthrorsResourceParameter authrorsResourceParameter)

        {

            var autors = _courseLibraryRepository.GetAuthors(authrorsResourceParameter);
            return new JsonResult(_mapper.Map<IEnumerable<AuthorDto>>(autors));
        }

        [HttpGet("{authorId}",Name ="GetAuthor")]
        public IActionResult GetAuthors(Guid authorId)
        {

            var autors = _courseLibraryRepository.GetAuthor(authorId);
            if (autors == null) return NotFound();
            return new JsonResult(_mapper.Map<AuthorDto>(autors));
        }

        [HttpPost]
        public ActionResult<AuthorDto> CreateAuthors(AuthorForCreationDto  authorForCreation)
        {
            var authorEntity = _mapper.Map<Author>(authorForCreation);
            _courseLibraryRepository.AddAuthor(authorEntity);
            _courseLibraryRepository.Save();
            var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtRoute("GetAuthor", new { authorId = authorToReturn.Id }, authorToReturn);


        }
    }
}
