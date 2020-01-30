using GoogleSmartHome.Models;

namespace GoogleSmartHome
{
    public interface ISmartHome
    {
        SmartHomeV1Payload Sync(string agentUserId);
        
        SmartHomeV1Payload Query(
            string agentUserId, 
            SmartHomeV1QueryRequestPayload payload);
 
        SmartHomeV1Payload Execute(
            string agentUserId,
            SmartHomeV1ExecuteRequestContext context, 
            SmartHomeV1ExecuteRequestPayload payload);

        SmartHomeV1Payload Disconnect(string agentUserId);
    }
}