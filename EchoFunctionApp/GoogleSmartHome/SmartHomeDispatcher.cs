using System;
using System.Linq;
using GoogleSmartHome.Models;
using Microsoft.Extensions.Logging;

namespace GoogleSmartHome
{
    public class SmartHomeDispatcher : ISmartHomeDispatcher
    {
        private readonly ILogger<SmartHomeDispatcher> _log;

        private readonly ISmartHome _smartHome;

        public SmartHomeDispatcher(ILogger<SmartHomeDispatcher> log, ISmartHome smartHome)
        {
            _log = log;
            _smartHome = smartHome;
        }

        public SmartHomeV1Response Invoke(string agentUserId, SmartHomeV1Request request)
        {
            if (agentUserId == null) throw new ArgumentNullException(nameof(agentUserId));
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (request.Inputs.Length != 1)
            {
                // Don't know why the request support batching of multiple inputs while
                // the responses are coupled to to the kind of intent.
                throw new NotSupportedException("Only one input is supported");
            }
            var input = request.Inputs.Single();
            
            _log.LogInformation($"Invoking intent {input.Intent}");


            return new SmartHomeV1Response
            {
                RequestId = request.RequestId,
                Payload = input.Intent switch
                {
                    SmartHomeV1Intents.Sync => _smartHome.Sync(agentUserId),
                    SmartHomeV1Intents.Query => _smartHome.Query(
                        agentUserId, 
                        input.Payload.ToObject<SmartHomeV1QueryRequestPayload>()),
                    SmartHomeV1Intents.Execute => _smartHome.Execute(
                        agentUserId,
                        input.Context.ToObject<SmartHomeV1ExecuteRequestContext>(),
                        input.Payload.ToObject<SmartHomeV1ExecuteRequestPayload>()),
                    SmartHomeV1Intents.Disconnect => _smartHome.Disconnect(agentUserId),
                    _ => throw new NotSupportedException($"Intent {input.Intent}")
                }
            };
        }
    }
}