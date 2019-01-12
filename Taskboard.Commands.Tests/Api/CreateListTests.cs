using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleInjector;
using Taskboard.Commands.Api;
using Taskboard.Commands.Commands;
using Taskboard.Commands.DTO;
using Taskboard.Commands.Handlers;

namespace Taskboard.Commands.Tests.Api
{
    [TestClass]
    public class CreateListTests
    {
        private static readonly TelemetryClient _telemetryClient = new TelemetryClient(new TelemetryConfiguration
        {
            DisableTelemetry = true
        });

        [TestMethod]
        public async Task Run_ReturnsUriOnSuccess()
        {
            Environment.SetEnvironmentVariable("LIST_RESOURCE_URI", "https://www.test.co.uk/{0}");

            var handler = new Mock<ICommandHander<CreateListCommand, string>>();
            var container = new Container();
            var list = new ListDTO {Name = "list"};
            var id = Guid.NewGuid().ToString();
            var expected = new Uri($"https://www.test.co.uk/{id}");

            handler.Setup(h => h.Execute(It.IsAny<CreateListCommand>())).ReturnsAsync(id);
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(_telemetryClient);
            CreateList.Container = container;

            var result = await CreateList.Run(list) as CreatedResult;

            Assert.IsNotNull(result);

            Assert.IsNull(result.Value);
            Assert.AreEqual(expected, result.Location);
        }

        [TestMethod]
        public async Task Run_ReturnsServerErrorOnError()
        {
            var handler = new Mock<ICommandHander<CreateListCommand, string>>();
            var container = new Container();
            var list = new ListDTO();

            handler.Setup(h => h.Execute(It.IsAny<CreateListCommand>())).ThrowsAsync(new Exception());
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(_telemetryClient);
            CreateList.Container = container;

            var result = await CreateList.Run(list) as InternalServerErrorResult;

            Assert.IsNotNull(result);
        }
    }
}