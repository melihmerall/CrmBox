using AutoMapper;

using CrmBox.Core.Domain;
using CrmBox.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Web.Mappings
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomVM>();
            CreateMap<RoomVM, Room>();
        }
    }
}
