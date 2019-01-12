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
using Taskboard.Commands.Exceptions;
using Taskboard.Commands.Handlers;

namespace Taskboard.Commands.Tests.Api
{
    [TestClass]
    public class CreateTaskTests
    {
        private static readonly TelemetryClient _telemetryClient = new TelemetryClient(new TelemetryConfiguration
        {
            DisableTelemetry = true
        });

        [TestMethod]
        public async Task Run_ReturnsUriOnSuccess()
        {
            Environment.SetEnvironmentVariable("TASK_RESOURCE_URI", "https://www.test.co.uk/{0}/task/{1}");

            var handler = new Mock<ICommandHander<CreateTaskCommand, string>>();
            var container = new Container();
            var task = new TaskDTO();
            var listId = Guid.NewGuid().ToString();
            var taskid = Guid.NewGuid().ToString();
            var expected = new Uri($"https://www.test.co.uk/{listId}/task/{taskid}");

            handler.Setup(h => h.Execute(It.IsAny<CreateTaskCommand>())).ReturnsAsync(taskid);
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(_telemetryClient);
            CreateTask.Container = container;

            var result = await CreateTask.Run(task, listId) as CreatedResult;

            Assert.IsNotNull(result);

            Assert.IsNull(result.Value);
            Assert.AreEqual(expected, result.Location);
        }

        [TestMethod]
        public async Task Run_ReturnsNotFoundOnNotFound()
        {
            var handler = new Mock<ICommandHander<CreateTaskCommand, string>>();
            var container = new Container();
            var task = new TaskDTO();
            var listId = Guid.NewGuid().ToString();

            handler.Setup(h => h.Execute(It.IsAny<CreateTaskCommand>()))
                .ThrowsAsync(new ResourceNotFoundException());
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(_telemetryClient);
            CreateTask.Container = container;

            var result = await CreateTask.Run(task, listId) as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Run_ReturnsServerErrorOnServerError()
        {
            var handler = new Mock<ICommandHander<CreateTaskCommand, string>>();
            var container = new Container();
            var task = new TaskDTO();
            var listId = Guid.NewGuid().ToString();

            handler.Setup(h => h.Execute(It.IsAny<CreateTaskCommand>()))
                .ThrowsAsync(new Exception());
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(_telemetryClient);
            CreateTask.Container = container;

            var result = await CreateTask.Run(task, listId) as InternalServerErrorResult;

            Assert.IsNotNull(result);
        }
    }
}