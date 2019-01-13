using System;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using SimpleInjector;
using Taskboard.Commands.Repositories;

namespace Taskboard.Commands.Extensions
{
    public static class ContainerExtensions
    {
        public static void WithTelemetryClient(this Container container)
        {
            container.RegisterSingleton(() => new TelemetryClient
            {
                InstrumentationKey = Environment.GetEnvironmentVariable("AI_INSTRUMENTATIONKEY")
            });
        }

        public static void WithDocumentClient(this Container container)
        {
            container.RegisterSingleton<IDocumentClient>(() =>
                new DocumentClient(
                    new Uri(Environment.GetEnvironmentVariable("COSMOS_ENDPOINT")),
                    Environment.GetEnvironmentVariable("COSMOS_KEY")
                )
            );
        }

        public static void WithListRepository(this Container container)
        {
            container.Register<IListRepository>(() => new ListRepository(container.GetInstance<IDocumentClient>(),
                Environment.GetEnvironmentVariable("COSMOS_DB"),
                Environment.GetEnvironmentVariable("COSMOS_COLLECTION")));
        }
    }
}