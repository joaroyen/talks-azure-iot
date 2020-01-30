using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

namespace EchoFunctionApp
{
    public class EchoFunction
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly IDeviceDirectMethod _deviceDirectMethod;

        public EchoFunction(
            JsonSerializerSettings jeJsonSerializerSettings,
            IDeviceDirectMethod deviceDirectMethod)
        {
            _jsonSerializerSettings = jeJsonSerializerSettings;
            _deviceDirectMethod = deviceDirectMethod;
        }

        /// <summary>
        ///NB! Update NuGet packages when using Visual Studio project template.
        /// </summary>
        [FunctionName("EchoFunction")]
        [Singleton] // Azure IoT Hub Free tier only support 1 concurrent job execution
        public async Task Run(
            [IoTHubTrigger("messages/events", 
                ConsumerGroup = "echo-function", 
                Connection = "iotHubEventHubCompatibleConnectionString")]
            EventData message, 
            ILogger log)
        {
            var messageBody = Encoding.UTF8.GetString(message.Body.Array);
            var counterUpdatedEvent = JsonConvert.DeserializeObject<CounterUpdatedEvent>(messageBody, _jsonSerializerSettings);
            if (!counterUpdatedEvent.IsValid())
            {
                log.LogWarning($"Received invalid message: {messageBody}");
                return;
            }

            log.LogInformation($"Received message: {messageBody}");

            await _deviceDirectMethod.Broadcast(counterUpdatedEvent);
        }
    }
}