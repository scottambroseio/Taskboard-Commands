using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleInjector;
using Taskboard.Commands.Api;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Exceptions;
using Taskboard.Commands.Handlers;

namespace Taskboard.Commands.Tests.Api
{
    [TestClass]
    public class DeleteListTests
    {
        private static readonly TelemetryClient _telemetryClient = new TelemetryClient(new TelemetryConfiguration
        {
            DisableTelemetry = true
        });

        [TestMethod]
        public async Task Run_ReturnsNoContentOnSuccess()
        {
            var handler = new Mock<ICommandHander<DeleteListCommand>>();
            var container = new Container();
            var id = Guid.NewGuid().ToString();
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            handler.Setup(h => h.Execute(It.IsAny<DeleteListCommand>())).Returns(Task.CompletedTask);
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(_telemetryClient);
            DeleteList.Container = container;

            var result = await DeleteList.Run(request, id) as NoContentResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Run_ReturnsNotFoundOnNotFound()
        {
            var handler = new Mock<ICommandHander<DeleteListCommand>>();
            var container = new Container();
            var id = Guid.NewGuid().ToString();
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            handler.Setup(h => h.Execute(It.IsAny<DeleteListCommand>()))
                .ThrowsAsync(new ResourceNotFoundException());
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(_telemetryClient);
            DeleteList.Container = container;

            var result = await DeleteList.Run(request, id) as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Run_ReturnsServerErrorOnServerError()
        {
            var handler = new Mock<ICommandHander<DeleteListCommand>>();
            var container = new Container();
            var id = Guid.NewGuid().ToString();
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            handler.Setup(h => h.Execute(It.IsAny<DeleteListCommand>()))
                .ThrowsAsync(new Exception());
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(_telemetryClient);
            DeleteList.Container = container;

            var result = await DeleteList.Run(request, id) as InternalServerErrorResult;

            Assert.IsNotNull(result);
        }
    }
}