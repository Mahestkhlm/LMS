using LMSLexicon20.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace LMSLexicon20
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<CourseClassViewModel, Course>();
        }
    }
}
