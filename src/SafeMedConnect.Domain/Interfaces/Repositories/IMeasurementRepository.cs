using SafeMedConnect.Domain.Entities;
using SafeMedConnect.Domain.Entities.Base;

namespace SafeMedConnect.Domain.Interfaces.Repositories;

public interface IMeasurementRepository<TA, TB> where TA : BaseObservationEntity<TB> where TB : BaseMeasurementEntity
{
    public Task<TA?> GetAsync(string userId, CancellationToken cnl = default);

    public Task<TA?> AddAsync(string userId, TB measurement, CancellationToken cnl = default);

    public Task<TA?> UpdateAsync(TA entity, CancellationToken cnl = default);
}