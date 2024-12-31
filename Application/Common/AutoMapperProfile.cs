using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Users.Commands;
using AutoMapper;
using Domain.Entities;

namespace Application.Common
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUserCommand, User>();
            CreateMap<User, CreateUserCommand>();
        }
        
    }
}