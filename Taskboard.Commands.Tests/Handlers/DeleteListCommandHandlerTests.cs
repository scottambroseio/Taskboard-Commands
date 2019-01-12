using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Handlers;
using Taskboard.Commands.Repositories;

namespace Taskboard.Commands.Tests.Handlers
{
    [TestClass]
    public class DeleteListCommandHandlerTests
    {
        [TestMethod]
        public void Execute_ReturnsCompletedTaskOnSuccess()
        {
            var repo = new Mock<IListRepository>();
            var id = Guid.NewGuid().ToString();
            var command = new DeleteListCommand {Id = id};
            var handler = new DeleteListCommandHandler(repo.Object);

            repo.Setup(r => r.Delete(It.IsAny<string>())).Returns(Task.CompletedTask);

            var result = handler.Execute(command);

            Assert.AreEqual(Task.CompletedTask, result);
        }
    }
}