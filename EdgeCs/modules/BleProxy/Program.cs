using System;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Transport.Mqtt;
using Newtonsoft.Json;

namespace BleProxy
{

    public class Program
    {
        private const string DeviceMethodName = "CounterUpdated";

        static MLTBT05BLEDevice _bleDevice;
        static ModuleClient _ioTHubModuleClient;

        static void Main(string[] args)
        {
            Init().Wait();

            BlockUntilUnloadOrCancelled().Wait();

            Cleanup().Wait();
        }

        private static async Task Init()
        {
            ITransportSettings[] settings = { new MqttTransportSettings(TransportType.Mqtt_Tcp_Only) };
            _ioTHubModuleClient = await ModuleClient.CreateFromEnvironmentAsync(settings);
            await _ioTHubModuleClient.OpenAsync();
            Console.WriteLine("IoT Hub module client initialized.");
            await _ioTHubModuleClient.SetMethodHandlerAsync(DeviceMethodName, CounterUpdatedMethod, _ioTHubModuleClient);

            _bleDevice = new MLTBT05BLEDevice(FromBleMessageHandler);
            await _bleDevice.Connect();
        }

        private static Task BlockUntilUnloadOrCancelled()
        {
            var cts = new CancellationTokenSource();
            AssemblyLoadContext.Default.Unloading += (ctx) => cts.Cancel();
            Console.CancelKeyPress += (sender, cpe) => cts.Cancel();
            var tcs = new TaskCompletionSource<bool>();
            cts.Token.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }

        private static async Task Cleanup()
        {
            await _bleDevice.Disconnect();

            await _ioTHubModuleClient.CloseAsync();
            _ioTHubModuleClient.Dispose();
            Console.WriteLine("Closed connection to IoT Hub");
        }

        private static async Task<MethodResponse> CounterUpdatedMethod(MethodRequest methodRequest, object userContext)
        {
            var counterUpdatedEvent = JsonConvert.DeserializeObject<CounterUpdatedEvent>(methodRequest.DataAsJson);
            Console.WriteLine($"Received event from {counterUpdatedEvent.Device}: Value={counterUpdatedEvent.Counter}");

            await _bleDevice.SendEvent(counterUpdatedEvent);

            return new MethodResponse(Encoding.UTF8.GetBytes("{\"message\":\"Counter updated event forwarded over BLE.\"}"), 0);
        }

        private static async Task FromBleMessageHandler(CounterUpdatedEvent counterUpdatedEvent)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(counterUpdatedEvent));

            var message = new Message(bytes);
            message.Properties.Add("DeviceName", counterUpdatedEvent.Device);
            message.Properties.Add("Value", counterUpdatedEvent.Counter.ToString());
            await _ioTHubModuleClient.SendEventAsync("counterUpdatedEvent", message);

            Console.WriteLine("Message sent as counterUpdatedEvent");
        }
    }
}
