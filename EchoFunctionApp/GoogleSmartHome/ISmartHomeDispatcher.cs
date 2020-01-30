using GoogleSmartHome.Models;

namespace GoogleSmartHome
{
    public interface ISmartHomeDispatcher
    {
        SmartHomeV1Response Invoke(string agentUserId, SmartHomeV1Request request);
    }
}