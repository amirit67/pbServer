using AutoMapper;
using Paye.Helpers;
using Paye.Models;
using Paye.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paye.Mappings
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageViewModel>()
                .ForMember(dst => dst.From, opt => opt.MapFrom(x => x.FromUser.Name/*DisplayName*/))
                .ForMember(dst => dst.To, opt => opt.MapFrom(x => x.ToRoom.Name))
                .ForMember(dst => dst.Avatar, opt => opt.MapFrom(x => x.FromUser.ProfileImage/*Avatar*/))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(x => BasicEmojis.ParseEmojis(x.Content)))
                .ForMember(dst => dst.Timestamp, opt => opt.MapFrom(x => new DateTime(long.Parse(x.Timestamp)).ToLongTimeString()));

            CreateMap<MessageViewModel, Message>();
        }
    }
}