﻿using AutoMapper;
using Coco.Framework.Models;
using Coco.Entities.Dtos.User;
using Coco.Entities.Dtos.Auth;
using System.Security.Claims;

namespace Coco.Framework.Infrastructure.MappingProfiles
{
    public class FrameworkMappingProfile : Profile
    {
        public FrameworkMappingProfile()
        {
            CreateMap<ApplicationUser, UserInfoModel>();
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed));
            CreateMap<UserDto, ApplicationUser>()
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.IsEmailConfirmed));
            CreateMap<UserFullDto, FullUserInfoModel>();
            CreateMap<ApplicationUser, UserIdentifierUpdateDto>();
            CreateMap<RoleDto, ApplicationRole>();
            CreateMap<UserRoleDto, ApplicationUserRole>();
            CreateMap<ApplicationUserClaim, UserClaimDto>();
            CreateMap<UserClaimDto, ApplicationUserClaim>();
            CreateMap<Claim, ClaimDto>();
            CreateMap<UserTokenDto, ApplicationUserToken>();
            CreateMap<ApplicationUserToken, UserTokenDto>();
            CreateMap<UserLoginDto, ApplicationUserLogin>();
            CreateMap<ApplicationUserLogin, UserLoginDto>();
        }
    }
}
