using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Domain;
using Taskboard.Commands.Exceptions;
using Taskboard.Commands.Handlers;
using Taskboard.Commands.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Taskboard.Commands.Tests.Handlers
{
    [TestClass]
    public class UpdateTaskCommandHandlerTests
    {
        [TestMethod]
        public async Task Execute_ReturnsCompletedTaskOnSuccess()
        {
            var repo = new Mock<IListRepository>();
            var listId = Guid.NewGuid().ToString();
            var taskId = Guid.NewGuid().ToString();
            var list = new List
            {
                Id = listId,
                Tasks = new List<Domain.Task>
                {
                    new Domain.Task
                    {
                        Id = taskId
                    }
                }
            };
            var command = new UpdateTaskCommand {ListId = listId, TaskId = taskId};
            var handler = new UpdateTaskCommandHandler(repo.Object);

            repo.Setup(r => r.GetById(It.IsAny<string>())).ReturnsAsync(list);

            var result = handler.Execute(command);

            await result;

            Assert.AreEqual(true, result.IsCompleted);
        }

        [TestMethod]
        public async Task Execute_ThrowsResourceNotFoundExceptionOnNoMatchingTask()
        {
            var repo = new Mock<IListRepository>();
            var listId = Guid.NewGuid().ToString();
            var taskId = Guid.NewGuid().ToString();
            var list = new List
            {
                Id = listId,
                Tasks = new List<Domain.Task>()
            };
            var command = new UpdateTaskCommand {ListId = listId, TaskId = taskId};
            var handler = new UpdateTaskCommandHandler(repo.Object);

            repo.Setup(r => r.GetById(It.IsAny<string>())).ReturnsAsync(list);

            await Assert.ThrowsExceptionAsync<ResourceNotFoundException>(() => handler.Execute(command));
        }
    }
}