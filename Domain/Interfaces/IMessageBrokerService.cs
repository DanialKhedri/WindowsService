using Domain.Entites;

namespace Domain.Interfaces;

public interface IMessageBrokerService
{

    Task SendDataAsync(List<DataPoint> dataPoints, CancellationToken cancellationToken = default);


}
