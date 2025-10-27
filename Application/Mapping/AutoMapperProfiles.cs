using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Responses;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class AutoMapperProfiles : AutoMapper.Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Activity, ActivityResponse>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<ActivitySession, FilterActivitySessionResponse>()
                .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(src => src.Activity.Name))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Activity.Category.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Activity.Description))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Activity.ImageUrl));
        }
    }
}
