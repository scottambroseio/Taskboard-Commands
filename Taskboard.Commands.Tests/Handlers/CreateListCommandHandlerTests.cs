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
    public class CreateListCommandHandlerTests
    {
        [TestMethod]
        public async Task Execute_ReturnsIdOnSuccess()
        {
            var repo = new Mock<IListRepository>();
            var id = Guid.NewGuid().ToString();
            var command = new CreateListCommand { Name = "test" };
            var handler = new CreateListCommandHandler(repo.Object);

            repo.Setup(r => r.Create(It.IsAny<List>())).ReturnsAsync(id);

            var result = await handler.Execute(command);

            Assert.AreEqual(id, result);
        }
    }
}