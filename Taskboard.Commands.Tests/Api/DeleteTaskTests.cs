using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Optional;
using SimpleInjector;
using Taskboard.Commands.Api;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Enums;
using Taskboard.Commands.Handlers;

namespace Taskboard.Commands.Tests.Api
{
    [TestClass]
    public class DeleteTaskTests
    {
        [TestMethod]
        public async Task ValidRequest_ReturnsCorrectResponse()
        {
            var handler = new Mock<ICommandHander<DeleteTaskCommand>>();
            var container = new Container();
            var listId = Guid.NewGuid().ToString();
            var taskId = Guid.NewGuid().ToString();
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            handler.Setup(h => h.Execute(It.IsAny<DeleteTaskCommand>()))
                .ReturnsAsync(Option.None<CommandFailure>());
            container.RegisterInstance(handler.Object);
            DeleteTask.Container = container;

            var result = await DeleteTask.Run(request, listId, taskId) as NoContentResult;

            Assert.IsNotNull(result);
        }
    }
}