using SafeMedConnect.Domain.Entities;

namespace SafeMedConnect.Domain.Interfaces.Repositories;

public interface IHeartRatesRepository
{
    public Task<HeartRateEntity?> AddHeartRateMeasurementAsync(
        string userId,
        HeartRateMeasurementEntity measurement,
        CancellationToken cnl = default
    );

    public Task<HeartRateEntity?> GetHeartRateMeasurementsAsync(
        string userId,
        CancellationToken cnl = default
    );
}