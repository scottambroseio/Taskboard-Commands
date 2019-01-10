using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SimpleInjector;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Handlers;
using Taskboard.Commands.Repositories;

namespace Taskboard.Commands.Api
{
    public static class DeleteList
    {
        public static Container Container = BuildContainer();

        [FunctionName(nameof(DeleteList))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "list/{id}")] HttpRequest req, string id)
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

            container.RegisterSingleton(() => new TelemetryClient
            {
                InstrumentationKey = Environment.GetEnvironmentVariable("AI_INSTRUMENTATIONKEY")
            });
            container.RegisterSingleton<IDocumentClient>(() =>
                new DocumentClient(new Uri(Environment.GetEnvironmentVariable("COSMOS_ENDPOINT")),
                    Environment.GetEnvironmentVariable("COSMOS_KEY")));
            container.Register<IListRepository>(() => new ListRepository(container.GetInstance<TelemetryClient>(),
                container.GetInstance<IDocumentClient>(),
                Environment.GetEnvironmentVariable("COSMOS_DB"),
                Environment.GetEnvironmentVariable("COSMOS_COLLECTION")));
            container.Register<ICommandHander<DeleteListCommand>, DeleteListCommandHandler>();

            return container;
        }
    }
}