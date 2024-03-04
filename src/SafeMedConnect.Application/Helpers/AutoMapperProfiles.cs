using AutoMapper;
using SafeMedConnect.Application.Commands.BloodPressure;
using SafeMedConnect.Application.Commands.BloodSugar;
using SafeMedConnect.Application.Commands.HeartRate;
using SafeMedConnect.Application.Commands.Temperature;
using SafeMedConnect.Application.Commands.User;
using SafeMedConnect.Application.Dto;
using SafeMedConnect.Application.Dto.Measurements;
using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Enums;

namespace SafeMedConnect.Application.Helpers;

internal sealed class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<UpdateUserInformationCommand, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<AddHeartRateMeasurementCommand, HeartRateMeasurementEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<AddBloodPressureCommand, BloodPressureMeasurementEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<AddTemperatureCommand, TemperatureMeasurementEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<AddBloodSugarCommand, BloodSugarMeasurementEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UserEntity, UserDto>();

        CreateMap<HeartRateMeasurementEntity, HeartRateDto>();
        CreateMap<BloodPressureMeasurementEntity, BloodPressureDto>();
        CreateMap<TemperatureMeasurementEntity, TemperatureDto>();
        CreateMap<BloodSugarMeasurementEntity, BloodSugarDto>()
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit.GetDisplayName()))
            .ForMember(dest => dest.MeasurementMethod, opt => opt.MapFrom(src => src.MeasurementMethod.GetDisplayName()));

        CreateMap<BloodSugarMeasurementEntity, BloodSugarMeasurementDto>()
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit.GetDisplayName()))
            .ForMember(dest => dest.MeasurementMethod, opt => opt.MapFrom(src => src.MeasurementMethod.GetDisplayName()));
    }
}