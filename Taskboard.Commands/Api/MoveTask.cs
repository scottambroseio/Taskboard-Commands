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
    public static class MoveTask
    {
        public static Container Container = BuildContainer();

        [FunctionName(nameof(MoveTask))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "list/{listid}/task/{taskid}/parent")]
            ParentDTO parent, string listid, string taskid)
        {
            try
            {
                var command = new MoveTaskCommand
                {
                    FromListId = listid,
                    ToListId = parent.Id,
                    TaskId = taskid
                };
                var handler = Container.GetInstance<ICommandHander<MoveTaskCommand>>();

                await handler.Execute(command);

                return new NoContentResult();
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
            container.Register<ICommandHander<MoveTaskCommand>, MoveTaskCommandHandler>();

            return container;
        }
    }
}