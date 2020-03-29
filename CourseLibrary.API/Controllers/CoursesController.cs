using AutoMapper;
using CourseLibrary.API.Entities;
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

        [HttpGet("{CourseId}", Name = "GetCourseForAuthor")]
        public ActionResult<CourseDto> GetCourseForAuthor(Guid authorId, Guid CourseId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId)) return NotFound();
            var Course = _courseLibraryRepository.GetCourse(authorId, CourseId);
            if (Course == null) return NotFound();

            return Ok(_mapper.Map<CourseDto>(Course));
        }

        [HttpPost()]
        public ActionResult<CourseDto> CreateCourseForAuthor(Guid authorId, CourseForCreationDto courseForCreation)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseEntity = _mapper.Map<Course>(courseForCreation);
            _courseLibraryRepository.AddCourse(authorId, courseEntity);
            _courseLibraryRepository.Save();
            var courseToReturn = _mapper.Map<CourseDto>(courseEntity);
            return CreatedAtAction("GetCourseForAuthor", new { authorId = authorId, CourseId = courseToReturn.Id }, courseToReturn);
        }


        [HttpPut("{courseId}")]
        public IActionResult UpdateCoursesForAuthor(Guid authorId, Guid courseId, CourseForUpdateDto course)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseForAuthorFromRep = _courseLibraryRepository.GetCourse(authorId, courseId);
            if (courseForAuthorFromRep == null)
            {
                var courseToAdd = _mapper.Map<Course>(course);
                courseToAdd.Id = courseId;
                _courseLibraryRepository.AddCourse(authorId,courseToAdd);
                _courseLibraryRepository.Save();
                var courseToReturn = _mapper.Map<CourseDto>(courseToAdd);
                return CreatedAtRoute("GetCourseForAuthor", new {  authorId, CourseId = courseToReturn.Id }, courseToReturn);
            }

            _mapper.Map(course, courseForAuthorFromRep);
            _courseLibraryRepository.UpdateCourse(courseForAuthorFromRep);
            _courseLibraryRepository.Save();
            return NoContent();
        }
    }
}

