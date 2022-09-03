using AutoMapper;
using Chat.Web.Helpers;
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
                .ForMember(dst => dst.Room, opt => opt.MapFrom(x => x.ToRoom.Name))
                .ForMember(dst => dst.Avatar, opt => opt.MapFrom(x => x.FromUser.Avatar))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(x => BasicEmojis.ParseEmojis(x.Content)));

            CreateMap<MessageVM, Message>();
        }
    }
}
