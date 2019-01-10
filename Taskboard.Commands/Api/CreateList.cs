using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SimpleInjector;
using Taskboard.Commands.Commands;
using Taskboard.Commands.DTO;
using Taskboard.Commands.Handlers;
using Taskboard.Commands.Repositories;

namespace Taskboard.Commands.Api
{
    public static class CreateList
    {
        public static Container Container = BuildContainer();

        [FunctionName(nameof(CreateList))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "lists")] ListDTO list)
        {
            var command = new CreateListCommand {Name = list.Name};
            var handler = Container.GetInstance<ICommandHander<CreateListCommand, Uri>>();

            var result = await handler.Execute(command);

            return result.Match<IActionResult>(
                location => new CreatedResult(location, null),
                error => new InternalServerErrorResult()
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
            container.Register<ICommandHander<CreateListCommand, Uri>, CreateListCommandHandler>();

            return container;
        }
    }
}