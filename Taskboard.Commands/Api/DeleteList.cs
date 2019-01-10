using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Handlers;

namespace Taskboard.Commands.Api
{
    public static class DeleteList
    {
        public static Container Container = BuildContainer();

        [FunctionName(nameof(DeleteList))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "list/{id}")] HttpRequest req, string id,
            ILogger log)
        {
            var command = new DeleteListCommand {Id = id};
            var handler = Container.GetInstance<ICommandHander<DeleteListCommand>>();

            var result = await handler.Execute(command);

            return result.Match<IActionResult>(
                error => new InternalServerErrorResult(),
                () => new NoContentResult()
            );
        }

        private static Container BuildContainer()
        {
            var container = new Container();

            container.Register<ICommandHander<DeleteListCommand>, DeleteListCommandHandler>();

            return container;
        }
    }
}