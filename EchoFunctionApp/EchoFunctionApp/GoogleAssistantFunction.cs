using System.IO;
using System.Threading.Tasks;
using GoogleSmartHome;
using GoogleSmartHome.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EchoFunctionApp
{
    public class GoogleAssistantFunction
    {
        private readonly ISmartHomeDispatcher _dispatcher;

        public GoogleAssistantFunction(ISmartHomeDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [FunctionName("GoogleAssistant")]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                // This trigger is protected by OAuth instead of keys
                AuthorizationLevel.Anonymous, 
                "get", "post", 
                Route = null)]
            HttpRequest request,
            ILogger log
            )
        {
            var agentUserId = request.HttpContext.User.Identity.Name;
            var body = await LoadBody(request);
            log.LogInformation(body.ToString());

            var result = _dispatcher.Invoke(
                agentUserId, 
                body.ToObject<SmartHomeV1Request>());
            return new OkObjectResult(result);
        }

        private static async Task<JObject> LoadBody(HttpRequest request)
        {
            using var streamReader = new StreamReader(request.Body);
            using var jsonReader = new JsonTextReader(streamReader);
            return await JObject.LoadAsync(jsonReader);
        }
    }
}