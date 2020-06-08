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
            CreateMap<User, UserListViewModel>()
                .ForMember(
                       dest => dest.FullName,
                       from => from.MapFrom(s => $"{s.FirstName} {s.LastName}"));
        }
    }
}
