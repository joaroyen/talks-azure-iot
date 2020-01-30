using System.Collections.Generic;
using EchoFunctionApp.Storage;
using GoogleSmartHome;
using GoogleSmartHome.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EchoFunctionApp
{
    /// <summary>
    /// This is a single user, single device implementation
    /// </summary>
    public class NnugSmartHome : ISmartHome
    {
        private const string DeviceId = "counter";

        private readonly ILogger<NnugSmartHome> _log;
        private readonly JsonSerializer _jsonSerializer;
        private readonly InMemoryDeviceStore _deviceStore;
        private readonly IDeviceDirectMethod _deviceDirectMethod;

        public NnugSmartHome(
            ILogger<NnugSmartHome> log, 
            JsonSerializer jsonSerializer,
            InMemoryDeviceStore deviceStore,
            IDeviceDirectMethod deviceDirectMethod)
        {
            _log = log;
            _jsonSerializer = jsonSerializer;
            _deviceStore = deviceStore;
            _deviceDirectMethod = deviceDirectMethod;
        }

        public SmartHomeV1Payload Sync(string agentUserId)
        {
            _log.LogInformation("Sync");

            return new SmartHomeV1SyncPayload
            {
                AgentUserId = agentUserId,
                Devices = new List<SmartHomeV1SyncDevices>
                {
                    new SmartHomeV1SyncDevices
                    {
                        Id = DeviceId,
                        Type = "action.devices.types.COFFEE_MAKER",
                        Traits = new List<string>
                        {
                            "action.devices.traits.TemperatureControl",
                            "action.devices.traits.OnOff"
                        },
                        Name = new SmartHomeV1SyncName
                        {
                            DefaultNames = new List<string>
                            {
                                "NNUG Teller"
                            },
                            Name = "Teller",
                            Nicknames = new List<string> { "teller" }
                        },
                        WillReportState = false,
                        DeviceInfo = new SmartHomeV1SyncDeviceInfo
                        {
                            Manufacturer = "Joar Øyen",
                            Model = "NNUG Teller",
                            HwVersion = "0.1",
                            SwVersion = "0.1"
                        },
                        Attributes = JObject.FromObject(
                            new TemperatureControlAttributes
                            {
                                TemperatureStepCelsius = 1.0m,
                                TemperatureRange = new TemperatureRange
                                {
                                    MinThresholdCelsius = -100,
                                    MaxThresholdCelsius = 100
                                }
                            },
                            _jsonSerializer)
                    }
                }
            };
        }


        public SmartHomeV1Payload Query(string agentUserId, SmartHomeV1QueryRequestPayload payload)
        {
            _log.LogInformation("Query");

            return new SmartHomeV1QueryPayload
            {
                Devices = new Dictionary<string, SmartHomeV1QueryDeviceBase>
                {
                    { 
                        DeviceId, 
                        _deviceStore.Counter.QueryResponse()
                    }
                }
            };
        }

        public SmartHomeV1Payload Execute(
            string agentUserId, 
            SmartHomeV1ExecuteRequestContext context,
            SmartHomeV1ExecuteRequestPayload payload)
        {
            _log.LogInformation("Execute");

            var response = new SmartHomeV1ExecutePayload();

            foreach (var command in payload.Commands)
            {
                // Ignoring device ids
                foreach (var execution in command.Execution)
                {
                    switch (execution.Command)
                    {
                        case "action.devices.commands.SetTemperature":
                            var temperature = execution.Params["temperature"].Value<decimal>();
                            _deviceStore.Counter.SetTemperature(temperature);
                            _deviceDirectMethod.Broadcast(_deviceStore.Counter.CounterUpdated());
                            break;
                        case "action.devices.commands.OnOff":
                            var on = execution.Params["on"].Value<bool>();
                            if (on)
                            {
                                _deviceStore.Counter.Increase();
                            }
                            else
                            {
                                _deviceStore.Counter.Decrease();
                            }

                            _deviceDirectMethod.Broadcast(_deviceStore.Counter.CounterUpdated());
                            break;
                    }
                }

                // TODO: Correlate command/executions
                response.Commands.Add(new SmartHomeV1ExecuteResponseCommands
                {
                    // Assuming all commands were executed without errors
                    Ids = new List<string>
                    {
                        DeviceId
                    },
                    Status = SmartHomeV1ExecuteStatus.Success,
                    States = _deviceStore.Counter.ExecuteResponseState()
                });
            }

            return response;
        }


        public SmartHomeV1Payload Disconnect(string agentUserId)
        {
            return new SmartHomeV1Payload();
        }
    }
}