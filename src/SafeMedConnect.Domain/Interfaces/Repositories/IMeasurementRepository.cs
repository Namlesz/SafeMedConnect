using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Domain.Interfaces.Repositories;

public interface IMeasurementRepository<TA, TB> where TA : BaseMeasurementEntity<TB> where TB : class
{
    public Task<TA?> GetAsync(string userId, CancellationToken cnl = default);

    public Task<TA?> AddAsync(string userId, TB measurement, CancellationToken cnl = default);

    public Task<TA?> UpdateAsync(TA entity, CancellationToken cnl = default);
}