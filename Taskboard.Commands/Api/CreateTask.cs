using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SimpleInjector;
using Taskboard.Commands.Commands;
using Taskboard.Commands.DTO;
using Taskboard.Commands.Exceptions;
using Taskboard.Commands.Extensions;
using Taskboard.Commands.Handlers;

namespace Taskboard.Commands.Api
{
    public static class CreateTask
    {
        public static Container Container = BuildContainer();

        [FunctionName(nameof(CreateTask))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "list/{listid}/tasks")] TaskDTO task,
            string listid)
        {
            try
            {
                var command = new CreateTaskCommand {ListId = listid, Name = task.Name, Description = task.Description};
                var handler = Container.GetInstance<ICommandHander<CreateTaskCommand, string>>();

                var id = await handler.Execute(command);

                var uri = new Uri(string.Format(Environment.GetEnvironmentVariable("TASK_RESOURCE_URI"), listid, id));

                return new CreatedResult(uri, null);
            }
            catch (ResourceNotFoundException ex)
            {
                Container.GetInstance<TelemetryClient>().TrackException(ex);

                return new NotFoundResult();
            }
            catch (Exception ex)
            {
                Container.GetInstance<TelemetryClient>().TrackException(ex);

                return new InternalServerErrorResult();
            }
        }

        private static Container BuildContainer()
        {
            var container = new Container();

            container.WithTelemetryClient();
            container.WithDocumentClient();
            container.WithListRepository();
            container.Register<ICommandHander<CreateTaskCommand, string>, CreateTaskCommandHandler>();

            return container;
        }
    }
}