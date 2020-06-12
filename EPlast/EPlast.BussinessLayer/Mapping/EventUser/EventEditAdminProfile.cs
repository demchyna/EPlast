﻿using AutoMapper;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BussinessLayer.Mapping.EventUser
{
    public class EventEditAdminProfile : Profile
    {
        public EventEditAdminProfile()
        {
            CreateMap<EventAdmin, CreateEventAdminDTO>().ReverseMap();
        }
    }
}
