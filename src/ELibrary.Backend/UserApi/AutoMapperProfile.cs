using Authentication.Models;
using AutoMapper;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;
using UserApi.Domain.Models;
using UserEntities.Domain.Entities;

namespace UserApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRegistrationRequest, User>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)) // Map Email to UserName
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));   // Map Email to Email

            CreateMap<UserUpdateDataRequest, UserUpdateModel>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<AdminUserRegistrationRequest, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<AdminUserUpdateDataRequest, UserUpdateModel>()
              .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
              .ForMember(dest => dest.OldPassword, opt => opt.MapFrom(src => src.Password));

            CreateMap<AccessTokenData, AuthToken>();
            CreateMap<AuthToken, AccessTokenData>();

            CreateMap<User, AdminUserResponse>();
        }
    }
}