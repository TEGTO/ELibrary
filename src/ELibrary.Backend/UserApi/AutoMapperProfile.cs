using Authentication.Models;
using AuthenticationApi.Domain.Dtos;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Domain.Models;
using AutoMapper;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Entities;

namespace AuthenticationApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserRegistrationRequest>();
            CreateMap<UserRegistrationRequest, User>();
            CreateMap<AccessTokenData, AuthToken>();
            CreateMap<AuthToken, AccessTokenData>();
            CreateMap<UserInfo, UserInfoDto>();
            CreateMap<UserInfoDto, UserInfo>();
            CreateMap<UserUpdateDataRequest, UserUpdateData>();
        }
    }
}