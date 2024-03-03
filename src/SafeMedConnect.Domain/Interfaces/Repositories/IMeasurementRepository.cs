using SafeMedConnect.Domain.Entities.Base;

namespace SafeMedConnect.Domain.Interfaces.Repositories;

public interface IMeasurementRepository<T> where T : BaseMeasurementEntity
{
    public Task<BaseObservationEntity<T>?> GetAsync(string userId, CancellationToken cnl = default);

    public Task<BaseObservationEntity<T>?> AddAsync(string userId, T measurement, CancellationToken cnl = default);

    public Task<BaseObservationEntity<T>?> UpdateAsync(BaseObservationEntity<T> entity, CancellationToken cnl = default);
}