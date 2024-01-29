using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Domain.Interfaces.Repositories;

public interface IMeasurementRepository<TA, TB> where TA : BaseMeasurementEntity<TB> where TB : class
{
    public Task<TA?> GetAsync(string userId, CancellationToken cnl = default);

    public Task<TA?> AddOrUpdateAsync(string userId, TB measurement, CancellationToken cnl = default);

    public Task<TA?> ReplaceAsync(TA entity, CancellationToken cnl = default);
}