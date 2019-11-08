using AutoMapper;
using Paye.Models;
using Paye.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paye.Mappings
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomViewModel>();

            CreateMap<RoomViewModel, Room>();
        }
    }
}