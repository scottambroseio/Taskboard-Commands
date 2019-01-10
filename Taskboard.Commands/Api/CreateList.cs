using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using Taskboard.Commands.Commands;
using Taskboard.Commands.DTO;
using Taskboard.Commands.Handlers;

namespace Taskboard.Commands.Api
{
    public static class CreateList
    {
        public static Container Container = BuildContainer();

        [FunctionName(nameof(CreateList))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "lists")] HttpRequest req, ListDTO list,
            ILogger log)
        {
            var command = new CreateListCommand {Name = list.Name};
            var handler = Container.GetInstance<ICommandHander<CreateListCommand, string>>();

            var result = await handler.Execute(command);

            return result.Match<IActionResult>(
                location => new CreatedResult(location, null),
                error => new InternalServerErrorResult()
            );
        }

        private static Container BuildContainer()
        {
            var container = new Container();

            container.Register<ICommandHander<CreateListCommand, string>, CreateListCommandHandler>();

            return container;
        }
    }
}