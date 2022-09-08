using AutoMapper;

using CrmBox.Core.Domain;
using CrmBox.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Web.Mappings
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageVM>()
                .ForMember(dst => dst.From, opt => opt.MapFrom(x => x.FromUser.FirstName))
               .ForMember(dst => dst.From, opt => opt.MapFrom(x => x.FromUser.LastName))
                .ForMember(dst => dst.Room, opt => opt.MapFrom(x => x.ToRoom.Name));


            CreateMap<MessageVM, Message>();
        }
    }
}
