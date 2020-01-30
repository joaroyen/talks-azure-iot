using System;
using System.Text;
using System.Threading.Tasks;
using EchoFunctionApp.Storage;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EchoFunctionApp
{
    public class DeviceDirectMethod : IDeviceDirectMethod
    {
        private long MaxExecutionTimeoutInSeconds = 10;
        private const string DeviceMethodName = "CounterUpdated";

        private readonly ILogger<DeviceDirectMethod> _log;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly EchoFunctionOptions _options;
        private readonly InMemoryDeviceStore _deviceStore;

        public DeviceDirectMethod(
            ILogger<DeviceDirectMethod> log,
            IOptions<EchoFunctionOptions> options,
            JsonSerializerSettings jsonSerializerSettings,
            InMemoryDeviceStore deviceStore)
        {
            _log = log;
            _jsonSerializerSettings = jsonSerializerSettings;
            _options = options.Value;
            _deviceStore = deviceStore;
        }

        public async Task Broadcast(CounterUpdatedEvent counterUpdatedEvent)
        {
            _deviceStore.Counter.Device = counterUpdatedEvent.Device;
            _deviceStore.Counter.Counter = counterUpdatedEvent.Counter;

            var jobClient = JobClient.CreateFromConnectionString(_options.IoTHubConnectionString);

            var cloudToDeviceMethod = new CloudToDeviceMethod(DeviceMethodName, TimeSpan.FromSeconds(5));
            cloudToDeviceMethod.SetPayloadJson(JsonConvert.SerializeObject(counterUpdatedEvent, _jsonSerializerSettings));
            
            await InvokeDeviceMethod(cloudToDeviceMethod, jobClient, "*");
            await InvokeDeviceMethod(cloudToDeviceMethod, jobClient, "FROM devices.modules WHERE devices.modules.moduleId = 'BleProxy'");
        }

        private async Task InvokeDeviceMethod(
            CloudToDeviceMethod cloudToDeviceMethod, 
            JobClient jobClient,
            string query)
        {
            _log.LogInformation($"Invoking {cloudToDeviceMethod.MethodName}...");
            var jobId = Guid.NewGuid().ToString();
            var response = await jobClient.ScheduleDeviceMethodAsync(
                jobId,
                query,
                cloudToDeviceMethod,
                DateTime.UtcNow,
                MaxExecutionTimeoutInSeconds);


            // Waiting for job to complete to avoid throttling
            while (response.Status != JobStatus.Completed
                   && response.Status != JobStatus.Failed)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                response = await jobClient.GetJobAsync(jobId);
            }

            if (response.Status == JobStatus.Completed)
            {
                _log.LogInformation($"Method {cloudToDeviceMethod.MethodName} completed");
            }
            else
            {
                _log.LogError(
                    $"Method {cloudToDeviceMethod.MethodName}, status:{response.Status}, status message:{response.StatusMessage}: failure reason: {response.FailureReason}");
            }
        }
    }
}