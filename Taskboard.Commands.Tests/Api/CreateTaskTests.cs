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
    public class CreateTaskTests
    {
        [TestMethod]
        public async Task ValidRequest_ReturnsCorrectResponse()
        {
            var handler = new Mock<ICommandHander<CreateTaskCommand>>();
            var container = new Container();
            var listId = Guid.NewGuid().ToString();
            var task = new TaskDTO {Name = "task", Description = "description"};

            handler.Setup(h => h.Execute(It.IsAny<CreateTaskCommand>()))
                .ReturnsAsync(Option.None<CommandFailure>());
            container.RegisterInstance(handler.Object);
            CreateTask.Container = container;

            var result = await CreateTask.Run(task, listId) as NoContentResult;

            Assert.IsNotNull(result);
        }
    }
}