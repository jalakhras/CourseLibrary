using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helper;
using CourseLibrary.API.Model;

namespace CourseLibrary.API.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(dist => dist.Name,
                opt => opt.MapFrom(src => $"{src.FirstName}{src.LastName}"))
                .ForMember(dist => dist.Age,
                opt => opt.MapFrom(src => src.DateOfBirth.GetCurrantAge()));



        }
    }
}
