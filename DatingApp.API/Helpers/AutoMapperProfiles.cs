using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<Users, UserForListDto>()
      .ForMember(dest => dest.PhotoUrl, opt =>
              opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
      .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));


      CreateMap<Users, UserForDetailDto>()
      .ForMember(dest => dest.PhotoUrl, opt =>
              opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
      .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

      CreateMap<UserForUpdateDto, Users>();
      CreateMap<Photo, PhotoForDetailDto>();
      CreateMap<PhotoToCreateDto, Photo>();
      CreateMap<Photo, PhotoForReturnDto>();

      CreateMap<UserForRegisterDto, Users>();

      CreateMap<MessageForCreationDto, Message>().ReverseMap();
      CreateMap<Message, MessageToReturnDto>()
          .ForMember(dest => dest.SenderKnownAs, opt => opt.MapFrom(src => src.Sender.KnownAs))
          .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
          .ForMember(dest => dest.RecipientKnownAs, opt => opt.MapFrom(src => src.Recipient.KnownAs))
          .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url))

          ;

    }
  }
}