using AutoMapper;
using CourseLibrary.API.Model;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CourseLibrary.API.Controllers
{
    [Route("api/authors/{authorId}/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;
        public CoursesController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ?? throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForAuthor(Guid authorId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId)) return NotFound();
            var Couses = _courseLibraryRepository.GetCourses(authorId);
            return Ok(_mapper.Map<IEnumerable<CourseDto>>(Couses));
        }

        [HttpGet("{CourseId}")]
        public ActionResult<CourseDto> GetCourseForAuthor(Guid authorId, Guid CourseId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId)) return NotFound();
            var Course = _courseLibraryRepository.GetCourse(authorId, CourseId);
            if (Course == null) return NotFound();

            return Ok(_mapper.Map<CourseDto>(Course));
        }
    }
}

