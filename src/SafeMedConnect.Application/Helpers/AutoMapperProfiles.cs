using AutoMapper;
using SafeMedConnect.Application.Commands.HeartRate;
using SafeMedConnect.Application.Commands.User;
using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Application.Helpers;

internal sealed class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<UpdateUserInformationCommand, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<AddHeartRateMeasurementCommand, HeartRateMeasurementEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}