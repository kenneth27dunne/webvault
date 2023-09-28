using AutoMapper;
using ClientApp.Models;
using ClientApp.Models.FirestoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Extensions
{
    internal class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Firebase.Auth.User, User>();
            CreateMap<FirebaseAdmin.Auth.UserRecord, User>();
            CreateMap<FirestoreUser, User>();
            CreateMap<Firebase.Auth.FirebaseAuthLink, AuthResponse>()
                .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.FirebaseToken));
        }
    }
}
