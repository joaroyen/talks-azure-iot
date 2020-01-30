using System.Threading.Tasks;

namespace EchoFunctionApp
{
    public interface IDeviceDirectMethod
    {
        Task Broadcast(CounterUpdatedEvent counterUpdatedEvent);
    }
}