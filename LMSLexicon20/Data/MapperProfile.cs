using AutoMapper;
using LMSLexicon20.Models;
using LMSLexicon20.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            //from viewmodel to user
            CreateMap<CreateUserViewModel, User>()
                .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.Email));
            CreateMap<User, UserListViewModel>()
                .ForMember(
                       dest => dest.FullName,
                       from => from.MapFrom(e => $"{e.FirstName} {e.LastName}"));

            CreateMap<User, UserDetailsViewModel>()
                .ForMember(
                       dest => dest.FullName,
                       from => from.MapFrom(e => $"{e.FirstName} {e.LastName}"));
            CreateMap<User, UserEditViewModel>().ReverseMap();

            CreateMap<Course, CourseIndexViewModel>();
            CreateMap<CreateCourseViewModel, Course>();


            CreateMap<Course, DeleteCourseViewModel>();

            CreateMap<CreateModuleViewModel, Module>();
            CreateMap<Module, DetailModuleViewModel>();
            CreateMap<Module, DeleteModuleViewModel>();
            CreateMap<Module, EditModuleViewModel>().ReverseMap();
            CreateMap<Module, IndexModuleViewModel>();



        }
    }
}
