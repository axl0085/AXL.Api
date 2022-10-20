using AutoMapper;
using AXL.Dto;

namespace AXL.Entitys.Base
{
    public class TransForMationProfile : Profile
    {
        public TransForMationProfile()
        {
            CreateMap<UserEntity, UserDto>().ReverseMap();
        }
    }
}