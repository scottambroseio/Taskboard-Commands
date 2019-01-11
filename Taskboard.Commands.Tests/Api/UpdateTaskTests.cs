using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Optional;
using SimpleInjector;
using Taskboard.Commands.Api;
using Taskboard.Commands.Commands;
using Taskboard.Commands.DTO;
using Taskboard.Commands.Enums;
using Taskboard.Commands.Handlers;

namespace Taskboard.Commands.Tests.Api
{
    [TestClass]
    public class UpdateTaskTests
    {
        [TestMethod]
        public async Task ValidRequest_ReturnsCorrectResponse()
        {
            var handler = new Mock<ICommandHander<UpdateTaskCommand>>();
            var container = new Container();
            var listId = Guid.NewGuid().ToString();
            var taskId = Guid.NewGuid().ToString();
            var task = new TaskDTO {Name = "task", Description = "description"};

            handler.Setup(h => h.Execute(It.IsAny<UpdateTaskCommand>()))
                .ReturnsAsync(Option.None<CommandFailure>());
            container.RegisterInstance(handler.Object);
            UpdateTask.Container = container;

            var result = await UpdateTask.Run(task, listId, taskId) as NoContentResult;

            Assert.IsNotNull(result);
        }
    }
}