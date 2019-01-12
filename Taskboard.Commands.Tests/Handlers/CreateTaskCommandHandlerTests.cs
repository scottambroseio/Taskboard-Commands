using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Domain;
using Taskboard.Commands.Handlers;
using Taskboard.Commands.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Taskboard.Commands.Tests.Handlers
{
    [TestClass]
    public class CreateTaskCommandHandlerTests
    {
        [TestMethod]
        public async Task Execute_ReturnsIdOnSuccess()
        {
            var repo = new Mock<IListRepository>();
            var listId = Guid.NewGuid().ToString();
            var command = new CreateTaskCommand {ListId = listId};
            var handler = new CreateTaskCommandHandler(repo.Object);

            repo.Setup(r => r.GetById(It.IsAny<string>())).ReturnsAsync(new List());

            var taskId = await handler.Execute(command);

            Assert.IsFalse(string.IsNullOrWhiteSpace(taskId));
            Assert.IsTrue(Guid.TryParse(taskId, out var _));
        }
    }
}