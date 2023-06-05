using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helper
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
            .ForMember(x => x.PhotoUrl, opt => opt.MapFrom(i => i.Photos.FirstOrDefault(src => src.IsMain).Url))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
        }
    }
}