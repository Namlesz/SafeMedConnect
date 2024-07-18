using AutoMapper;
using SafeMedConnect.Domain.Abstract.Repositories;
using SafeMedConnect.Domain.Entities.Base;
using SafeMedConnect.Domain.Enums;
using SafeMedConnect.Domain.Models;

namespace SafeMedConnect.Application.Factories;

internal sealed class MeasurementFactory<T>(
    IMeasurementRepository<T> repository,
    string userId
) where T : BaseMeasurementEntity
{
    public async Task<ApiResponse<List<T>>> GetMeasurementsAsync(
        CancellationToken cancellationToken = default
    )
    {
        var entity = await repository.GetAsync(userId, cancellationToken);

        return entity?.Measurements is null
            ? new ApiResponse<List<T>>(ApiResponseTypes.NotFound)
            : new ApiResponse<List<T>>(ApiResponseTypes.Success, entity.Measurements);
    }

    public async Task<ApiResponse<List<T>>> AddMeasurementAsync(
        T measurementEntity,
        CancellationToken cancellationToken = default
    )
    {
        var result = await repository.AddAsync(userId, measurementEntity, cancellationToken);
        return result?.Measurements is null
            ? new ApiResponse<List<T>>(
                ApiResponseTypes.Error,
                "Error while adding measurement"
            )
            : new ApiResponse<List<T>>(
                ApiResponseTypes.Success,
                result.Measurements
            );
    }

    public async Task<ApiResponse<List<T>>> DeleteMeasurementAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await repository.GetAsync(userId, cancellationToken);
        if (entity?.Measurements is null)
        {
            return new ApiResponse<List<T>>(ApiResponseTypes.Error);
        }

        var measurementToDelete = entity.Measurements.FirstOrDefault(x => x.Id == id);
        if (measurementToDelete is null)
        {
            return new ApiResponse<List<T>>(ApiResponseTypes.NotFound);
        }

        entity.Measurements.Remove(measurementToDelete);

        var result = await repository.UpdateAsync(entity, cancellationToken);
        return result?.Measurements is null
            ? new ApiResponse<List<T>>(
                ApiResponseTypes.Error,
                "Error while deleting measurement"
            )
            : new ApiResponse<List<T>>(
                ApiResponseTypes.Success,
                result.Measurements
            );
    }
}

internal sealed class MeasurementFactoryWithMapper<T>(
    IMeasurementRepository<T> repository,
    string userId,
    IMapper mapper
) where T : BaseMeasurementEntity
{
    public async Task<ApiResponse<List<TB>>> GetMeasurementsAsync<TB>(
        CancellationToken cancellationToken = default
    ) where TB : class
    {
        var entity = await repository.GetAsync(userId, cancellationToken);

        return entity?.Measurements is null
            ? new ApiResponse<List<TB>>(ApiResponseTypes.NotFound)
            : new ApiResponse<List<TB>>(ApiResponseTypes.Success, mapper.Map<List<TB>>(entity.Measurements));
    }

    public async Task<ApiResponse<List<TB>>> AddMeasurementAsync<TB>(
        T measurementEntity,
        CancellationToken cancellationToken = default
    ) where TB : class
    {
        var result = await repository.AddAsync(userId, measurementEntity, cancellationToken);
        return result?.Measurements is null
            ? new ApiResponse<List<TB>>(
                ApiResponseTypes.Error,
                "Error while adding measurement"
            )
            : new ApiResponse<List<TB>>(
                ApiResponseTypes.Success,
                mapper.Map<List<TB>>(result.Measurements)
            );
    }

    public async Task<ApiResponse<List<TB>>> DeleteMeasurementAsync<TB>(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await repository.GetAsync(userId, cancellationToken);
        if (entity?.Measurements is null)
        {
            return new ApiResponse<List<TB>>(ApiResponseTypes.Error);
        }

        var measurementToDelete = entity.Measurements.FirstOrDefault(x => x.Id == id);
        if (measurementToDelete is null)
        {
            return new ApiResponse<List<TB>>(ApiResponseTypes.NotFound);
        }

        entity.Measurements.Remove(measurementToDelete);

        var result = await repository.UpdateAsync(entity, cancellationToken);
        return result?.Measurements is null
            ? new ApiResponse<List<TB>>(
                ApiResponseTypes.Error,
                "Error while deleting measurement"
            )
            : new ApiResponse<List<TB>>(
                ApiResponseTypes.Success,
                mapper.Map<List<TB>>(result.Measurements)
            );
    }
}