using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashtagChris.DotNetBlueZ;
using HashtagChris.DotNetBlueZ.Extensions;

namespace BleProxy
{
    public class MLTBT05BLEDevice
    {
        // Hard coded device address.
        private const string DeviceAddress = "C8:FD:19:66:DC:B5";
        private const string ServiceUUID = "ffe0";
        private const string CharacteristicUUID = "ffe1";


        private Adapter _adapter;
        private Device _device;
        private GattCharacteristic _characteristic;
        private Func<CounterUpdatedEvent, Task> _eventHandler;

        public MLTBT05BLEDevice(Func<CounterUpdatedEvent, Task> eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public async Task Connect()
        {
            var adapters = await BlueZManager.GetAdaptersAsync();
            if (adapters.Count == 0)
            {
                Console.WriteLine("No BlueTooth adapters found");
            }
            _adapter = adapters.First();

            _device = await _adapter.GetDeviceAsync(DeviceAddress);
            _device.Disconnected += Reconnect;
            
            await _device.ConnectAsync();
            Console.WriteLine($"Connected to device {await _device.GetNameAsync()} on {await _adapter.GetNameAsync()}/{DeviceAddress}");


            var service = await _device.GetServiceAsync(BlueZManager.NormalizeUUID(ServiceUUID));
            _characteristic = await service.GetCharacteristicAsync(BlueZManager.NormalizeUUID(CharacteristicUUID));

            _characteristic.Value += CharacteristicOnValue;

        }

        public async Task SendEvent(CounterUpdatedEvent counterUpdatedEvent)
        {
            await _characteristic.WriteValueAsync(
                Encoding.ASCII.GetBytes($"{counterUpdatedEvent.Device};{1};{counterUpdatedEvent.Counter}\r\n"),
                new Dictionary<string, object>());
        }

        public async Task Disconnect()
        {
            await _characteristic.StopNotifyAsync();
            await _device.DisconnectAsync();
            
            _characteristic.Dispose();
            _device.Dispose();
            _adapter.Dispose();
            
            Console.WriteLine("Disconnected from BLE device");
        }

        private async Task Reconnect(Device sender, BlueZEventArgs eventArgs)
        {
            while (!(await sender.GetConnectedAsync()))
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
                await sender.ConnectAsync();
            }

            Console.WriteLine("Reconnected to BLE device");
        }

        private async Task CharacteristicOnValue(GattCharacteristic sender, GattCharacteristicValueEventArgs args)
        {
            var eventContent = Encoding.UTF8.GetString(args.Value);
            Console.WriteLine($"{DateTimeOffset.Now}: Received event over BLE: {eventContent}");

            var eventParts = eventContent.Split(';');
            var counterUpdatedEvent = new CounterUpdatedEvent
            {
                Device = eventParts[0],
                Counter = int.Parse(eventParts[2])
            };

            await _eventHandler(counterUpdatedEvent);

            await Task.CompletedTask;
        }
    }
}