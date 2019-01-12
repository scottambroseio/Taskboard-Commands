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
            try
            {
                var command = new CreateListCommand {Name = list.Name};
                var handler = Container.GetInstance<ICommandHander<CreateListCommand, string>>();

                var id = await handler.Execute(command);

                var uri = new Uri(string.Format(Environment.GetEnvironmentVariable("LIST_RESOURCE_URI"), id));

                return new CreatedResult(uri, null);
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

            container.RegisterSingleton(() => new TelemetryClient
            {
                InstrumentationKey = Environment.GetEnvironmentVariable("AI_INSTRUMENTATIONKEY")
            });
            container.RegisterSingleton<IDocumentClient>(() =>
                new DocumentClient(new Uri(Environment.GetEnvironmentVariable("COSMOS_ENDPOINT")),
                    Environment.GetEnvironmentVariable("COSMOS_KEY")));
            container.Register<IListRepository>(() => new ListRepository(container.GetInstance<IDocumentClient>(),
                Environment.GetEnvironmentVariable("COSMOS_DB"),
                Environment.GetEnvironmentVariable("COSMOS_COLLECTION")));
            container.Register<ICommandHander<CreateListCommand, string>, CreateListCommandHandler>();

            return container;
        }
    }
}