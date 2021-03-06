﻿using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.WebApi.Models.Decision;

namespace EPlast.WebApi.Mapping.Decision
{
    public class DecisionProfile : Profile
    {
        public DecisionProfile()
        {
            CreateMap<DecisionDTO, DecisionViewModel>()
                .ForMember(dvw => dvw.Organization, dd => dd.MapFrom(f => f.Organization.OrganizationName))
                .ForMember(dvw => dvw.DecisionTarget, dd => dd.MapFrom(f => f.DecisionTarget.TargetName))
                .ForMember(dvw => dvw.DecisionStatusType, dd => dd.MapFrom(f => f.DecisionTarget.TargetName))
                .ForMember(dvw => dvw.Date, dd => dd.MapFrom(f => f.Date.ToString("MM.dd.yyyy")))
                .ReverseMap();
        }
    }
}
