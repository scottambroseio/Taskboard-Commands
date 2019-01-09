using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Taskboard.Commands.Api
{
    public static class Example
    {
        [FunctionName(nameof(Example))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "example")] HttpRequest req, ILogger log)
        {
            return new OkObjectResult("Hello from Taskboard.Commands.Api");
        }
    }
}