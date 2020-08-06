using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helper;
using CourseLibrary.API.Model;
using CourseLibrary.API.ResourceParameter;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _propertyCheckerService;
        public AuthorController(ICourseLibraryRepository courseLibraryRepository, IPropertyMappingService propertyMappingService, IMapper mapper, IPropertyCheckerService propertyCheckerService)
        {
            _courseLibraryRepository = courseLibraryRepository ?? throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService = propertyMappingService;
            _propertyCheckerService = propertyCheckerService;
        }
        //[HttpGet()]
        //[HttpHead]
        //public ActionResult<IEnumerable<AuthorDto>> GetAuthors()

        //{

        //    var autors = _courseLibraryRepository.GetAuthors();
        //    return new JsonResult(_mapper.Map<IEnumerable<AuthorDto>>(autors));
        //}
        [HttpGet(Name = "GetAuthors")]
        public IActionResult GetAuthors([FromQuery] AuthorsResourceParameters authorsResourceParameters)

        {
            if (!_propertyMappingService.ValidMappingExistsFor<AuthorDto, Author>(authorsResourceParameters.OrderBy) || !_propertyCheckerService.TypeHasProperties<AuthorDto>(authorsResourceParameters.Fields))
            {
                return BadRequest();
            }

            var authorsFromRepo = _courseLibraryRepository.GetAuthors(authorsResourceParameters);
         
            var paginationMetadata = new
            {
                totalCount = authorsFromRepo.TotalCount,
                pageSize = authorsFromRepo.PageSize,
                currentPage = authorsFromRepo.CurrentPage,
                totalPages = authorsFromRepo.TotalPages,
               
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));
            var links = CreateLinksForAuthors(authorsResourceParameters, authorsFromRepo.HasNext, authorsFromRepo.HasPrevious);
            var shapedAuthors = _mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo).ShapeData(authorsResourceParameters.Fields);

            var shapedAuthorsWithLinks = shapedAuthors.Select(author =>
            {
                var authorAsDictionary = author as IDictionary<string, object>;
                var authorLinks = CreateLinksForAuthor((Guid)authorAsDictionary["Id"], null);
                authorAsDictionary.Add("links", authorLinks);
                return authorAsDictionary;
            });

            var linkedCollectionResource = new
            {
                value = shapedAuthorsWithLinks,
                links
            };

            return Ok(linkedCollectionResource);
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public IActionResult GetAuthors(Guid authorId, string fields)
        {
            if (!_propertyCheckerService.TypeHasProperties<AuthorDto>(fields))
            {
                return BadRequest();
            }
            var autors = _courseLibraryRepository.GetAuthor(authorId);
            if (autors == null) return NotFound();
            var links = CreateLinksForAuthor(authorId, fields);
            var linkedResourceToReturn = _mapper.Map<AuthorDto>(autors).ShapeData(fields)
               as IDictionary<string, object>;
            linkedResourceToReturn.Add("links", links);
            return Ok(linkedResourceToReturn);

        }

        [HttpPost(Name = "CreateAuthor")]
        public ActionResult<AuthorDto> CreateAuthors(AuthorForCreationDto authorForCreation)
        {
            var authorEntity = _mapper.Map<Author>(authorForCreation);
            _courseLibraryRepository.AddAuthor(authorEntity);
            _courseLibraryRepository.Save();
            var linkedResourceToReturn = _mapper.Map<AuthorDto>(authorEntity).ShapeData(null) as IDictionary<string,object>;
            var links = CreateLinksForAuthor(authorEntity.Id, null);
            linkedResourceToReturn.Add("links", links);
            return CreatedAtRoute("GetAuthor", new { authorId = linkedResourceToReturn["Id"] }, linkedResourceToReturn);


        }



        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        [HttpDelete("{authorId}" ,Name = "DeleteAuthor")]
        public ActionResult DeleteAuthor(Guid authorId)
        {
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            _courseLibraryRepository.DeleteAuthor(authorFromRepo);

            _courseLibraryRepository.Save();

            return NoContent();
        }

        private string CreateAuthorsResourceUri(
             AuthorsResourceParameters authorsResourceParameters,
             ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetAuthors",
                      new

                      {
                          fields = authorsResourceParameters.Fields,
                          orderBy = authorsResourceParameters.OrderBy,
                          pageNumber = authorsResourceParameters.PageNumber - 1,
                          pageSize = authorsResourceParameters.PageSize,
                          mainCategory = authorsResourceParameters.MainCategory,
                          searchQuery = authorsResourceParameters.SearchQuery
                      });
                case ResourceUriType.NextPage:
                    return Url.Link("GetAuthors",
                      new
                      {
                          fields = authorsResourceParameters.Fields,
                          orderBy = authorsResourceParameters.OrderBy,
                          pageNumber = authorsResourceParameters.PageNumber + 1,
                          pageSize = authorsResourceParameters.PageSize,
                          mainCategory = authorsResourceParameters.MainCategory,
                          searchQuery = authorsResourceParameters.SearchQuery
                      });
                case ResourceUriType.Current:
                default:
                    return Url.Link("GetAuthors",
                    new
                    {
                        fields = authorsResourceParameters.Fields,
                        orderBy = authorsResourceParameters.OrderBy,
                        pageNumber = authorsResourceParameters.PageNumber,
                        pageSize = authorsResourceParameters.PageSize,
                        mainCategory = authorsResourceParameters.MainCategory,
                        searchQuery = authorsResourceParameters.SearchQuery
                    });
            }

        }
        private IEnumerable<LinkDto> CreateLinksForAuthor(Guid authorId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(Url.Link("GetAuthor", new { authorId }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(Url.Link("GetAuthor", new { authorId, fields }),
                  "self",
                  "GET"));
            }

            links.Add(
               new LinkDto(Url.Link("DeleteAuthor", new { authorId }),
               "delete_author",
               "DELETE"));

            links.Add(
                new LinkDto(Url.Link("CreateCourseForAuthor", new { authorId }),
                "create_course_for_author",
                "POST"));

            links.Add(
               new LinkDto(Url.Link("GetCoursesForAuthor", new { authorId }),
               "courses",
               "GET"));

            return links;
        }
        private IEnumerable<LinkDto> CreateLinksForAuthors(AuthorsResourceParameters authorsResourceParameters,bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateAuthorsResourceUri(
                   authorsResourceParameters, ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateAuthorsResourceUri(
                      authorsResourceParameters, ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateAuthorsResourceUri(
                        authorsResourceParameters, ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }
    }

}
