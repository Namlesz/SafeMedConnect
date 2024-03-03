using SafeMedConnect.Domain.Entities.Base;

namespace SafeMedConnect.Domain.Interfaces.Repositories;

public interface IMeasurementRepository<TA, TB> where TA : BaseObservationEntity<TB> where TB : BaseMeasurementEntity
{
    public Task<TA?> GetAsync(string userId, CancellationToken cnl = default);

    public Task<TA?> AddAsync(string userId, TB measurement, CancellationToken cnl = default);

    public Task<TA?> UpdateAsync(TA entity, CancellationToken cnl = default);
}

public interface IMeasurementRepositorySimplified<T> where T : BaseMeasurementEntity
{
    public Task<BaseObservationEntity<T>?> GetAsync(string userId, CancellationToken cnl = default);

    public Task<BaseObservationEntity<T>?> AddAsync(string userId, T measurement, CancellationToken cnl = default);

    public Task<BaseObservationEntity<T>?> UpdateAsync(BaseObservationEntity<T> entity, CancellationToken cnl = default);
}