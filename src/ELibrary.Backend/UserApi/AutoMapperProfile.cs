using Authentication.Models;
using UserApi.Domain.Dtos;
using UserApi.Domain.Entities;
using AutoMapper;
using UserApi.Domain.Dtos;
using UserApi.Domain.Entities;

namespace UserApi
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
        }
    }
}