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
    public class UpdateTaskTests
    {
        private static readonly TelemetryClient _telemetryClient = new TelemetryClient(new TelemetryConfiguration
        {
            DisableTelemetry = true
        });

        [TestMethod]
        public async Task Run_ReturnsNoContentOnSuccess()
        {
            var handler = new Mock<ICommandHander<UpdateTaskCommand>>();
            var container = new Container();
            var task = new TaskDTO();
            var listId = Guid.NewGuid().ToString();
            var taskId = Guid.NewGuid().ToString();

            handler.Setup(h => h.Execute(It.IsAny<UpdateTaskCommand>())).Returns(Task.CompletedTask);
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(_telemetryClient);
            UpdateTask.Container = container;

            var result = await UpdateTask.Run(task, listId, taskId) as NoContentResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Run_ReturnsNotFoundOnNotFound()
        {
            var handler = new Mock<ICommandHander<UpdateTaskCommand>>();
            var container = new Container();
            var task = new TaskDTO();
            var listId = Guid.NewGuid().ToString();
            var taskId = Guid.NewGuid().ToString();

            handler.Setup(h => h.Execute(It.IsAny<UpdateTaskCommand>()))
                .ThrowsAsync(new ResourceNotFoundException());
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(_telemetryClient);
            UpdateTask.Container = container;

            var result = await UpdateTask.Run(task, listId, taskId) as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Run_ReturnsServerErrorOnServerError()
        {
            var handler = new Mock<ICommandHander<UpdateTaskCommand>>();
            var container = new Container();
            var task = new TaskDTO();
            var listId = Guid.NewGuid().ToString();
            var taskId = Guid.NewGuid().ToString();

            handler.Setup(h => h.Execute(It.IsAny<UpdateTaskCommand>()))
                .ThrowsAsync(new Exception());
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(_telemetryClient);
            UpdateTask.Container = container;

            var result = await UpdateTask.Run(task, listId, taskId) as InternalServerErrorResult;

            Assert.IsNotNull(result);
        }
    }
}