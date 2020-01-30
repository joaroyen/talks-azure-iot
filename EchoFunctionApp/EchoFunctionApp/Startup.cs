using System.IO;
using EchoFunctionApp.Storage;
using GoogleSmartHome;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

[assembly: FunctionsStartup(typeof(EchoFunctionApp.Startup))]
namespace EchoFunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.Configure<EchoFunctionOptions>(config);
            builder.Services.AddSingleton(config);

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsonSerializerSettings.Converters.Add(new StringEnumConverter());
            builder.Services.AddSingleton(jsonSerializerSettings);
            builder.Services.AddSingleton(JsonSerializer.Create(jsonSerializerSettings));

            builder.Services.AddSingleton<InMemoryDeviceStore>();
            builder.Services.AddScoped<IDeviceDirectMethod, DeviceDirectMethod>();
            builder.Services.AddScoped<ISmartHome, NnugSmartHome>();
            builder.Services.AddScoped<ISmartHomeDispatcher, SmartHomeDispatcher>();
        }
    }
}