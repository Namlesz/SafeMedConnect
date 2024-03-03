using SafeMedConnect.Domain.Entities.Base;
using SafeMedConnect.Domain.Interfaces.Repositories;
using SafeMedConnect.Domain.Interfaces.Services;
using SafeMedConnect.Domain.Responses;

namespace SafeMedConnect.Application.Factories;

// TODO: Maybe better to provide userId as a parameter to the factory methods
internal sealed class MeasurementFactory<TA, TB>(
    ISessionService session,
    IMeasurementRepository<TA, TB> repository
) where TA : BaseObservationEntity<TB> where TB : BaseMeasurementEntity
{
    public async Task<ResponseWrapper<List<TB>>> GetMeasurementsAsync(
        CancellationToken cancellationToken = default
    )
    {
        var userId = session.GetUserClaims().UserId;
        var entity = await repository.GetAsync(userId, cnl: cancellationToken);

        return entity?.Measurements is null
            ? new ResponseWrapper<List<TB>>(ResponseTypes.NotFound)
            : new ResponseWrapper<List<TB>>(ResponseTypes.Success, data: entity.Measurements);
    }

    public async Task<ResponseWrapper<List<TB>>> AddMeasurementAsync(
        TB measurementEntity,
        CancellationToken cancellationToken = default
    )
    {
        var userId = session.GetUserClaims().UserId;

        var result = await repository.AddAsync(userId, measurementEntity, cancellationToken);
        return result?.Measurements is null
            ? new ResponseWrapper<List<TB>>(
                ResponseTypes.Error,
                "Error while adding measurement"
            )
            : new ResponseWrapper<List<TB>>(
                ResponseTypes.Success,
                data: result.Measurements
            );
    }

    public async Task<ResponseWrapper<List<TB>>> DeleteMeasurementAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var userId = session.GetUserClaims().UserId;

        var entity = await repository.GetAsync(userId, cancellationToken);
        if (entity?.Measurements is null)
        {
            return new ResponseWrapper<List<TB>>(ResponseTypes.Error);
        }

        var measurementToDelete = entity.Measurements.FirstOrDefault(x => x.Id == id);
        if (measurementToDelete is null)
        {
            return new ResponseWrapper<List<TB>>(ResponseTypes.NotFound);
        }

        entity.Measurements.Remove(measurementToDelete);

        var result = await repository.UpdateAsync(entity, cancellationToken);
        return result?.Measurements is null
            ? new ResponseWrapper<List<TB>>(
                ResponseTypes.Error,
                "Error while deleting measurement"
            )
            : new ResponseWrapper<List<TB>>(
                ResponseTypes.Success,
                data: result.Measurements
            );
    }
}

internal sealed class MeasurementFactorySimplified<T>(
    IMeasurementRepositorySimplified<T> repository,
    string userId
) where T : BaseMeasurementEntity
{
    public async Task<ResponseWrapper<List<T>>> GetMeasurementsAsync(
        CancellationToken cancellationToken = default
    )
    {
        var entity = await repository.GetAsync(userId, cnl: cancellationToken);

        return entity?.Measurements is null
            ? new ResponseWrapper<List<T>>(ResponseTypes.NotFound)
            : new ResponseWrapper<List<T>>(ResponseTypes.Success, data: entity.Measurements);
    }

    public async Task<ResponseWrapper<List<T>>> AddMeasurementAsync(
        T measurementEntity,
        CancellationToken cancellationToken = default
    )
    {
        var result = await repository.AddAsync(userId, measurementEntity, cancellationToken);
        return result?.Measurements is null
            ? new ResponseWrapper<List<T>>(
                ResponseTypes.Error,
                "Error while adding measurement"
            )
            : new ResponseWrapper<List<T>>(
                ResponseTypes.Success,
                data: result.Measurements
            );
    }

    public async Task<ResponseWrapper<List<T>>> DeleteMeasurementAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await repository.GetAsync(userId, cancellationToken);
        if (entity?.Measurements is null)
        {
            return new ResponseWrapper<List<T>>(ResponseTypes.Error);
        }

        var measurementToDelete = entity.Measurements.FirstOrDefault(x => x.Id == id);
        if (measurementToDelete is null)
        {
            return new ResponseWrapper<List<T>>(ResponseTypes.NotFound);
        }

        entity.Measurements.Remove(measurementToDelete);

        var result = await repository.UpdateAsync(entity, cancellationToken);
        return result?.Measurements is null
            ? new ResponseWrapper<List<T>>(
                ResponseTypes.Error,
                "Error while deleting measurement"
            )
            : new ResponseWrapper<List<T>>(
                ResponseTypes.Success,
                data: result.Measurements
            );
    }
}