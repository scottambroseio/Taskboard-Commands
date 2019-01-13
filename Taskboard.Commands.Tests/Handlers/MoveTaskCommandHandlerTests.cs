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
    public class MoveTaskCommandHandlerTests
    {
        [TestMethod]
        public async Task Execute_ReturnsCompletedTaskOnSuccess()
        {
            var repo = new Mock<IListRepository>();
            var taskId = Guid.NewGuid().ToString();
            var fromListId = Guid.NewGuid().ToString();
            var toListId = Guid.NewGuid().ToString();

            var fromList = new List
            {
                Id = fromListId,
                Tasks = new List<Domain.Task>
                {
                    new Domain.Task
                    {
                        Id = taskId
                    }
                }
            };

            var toList = new List
            {
                Id = fromListId,
                Tasks = new List<Domain.Task>()
            };

            var command = new MoveTaskCommand {FromListId = fromListId, ToListId = toListId, TaskId = taskId};
            var handler = new MoveTaskCommandHandler(repo.Object);

            repo.Setup(r => r.GetById(fromListId)).ReturnsAsync(fromList);
            repo.Setup(r => r.GetById(toListId)).ReturnsAsync(toList);

            var result = handler.Execute(command);

            await result;

            Assert.AreEqual(true, result.IsCompleted);
        }

        [TestMethod]
        public async Task Execute_MovesTaskOnSuccess()
        {
            var repo = new Mock<IListRepository>();
            var taskId = Guid.NewGuid().ToString();
            var fromListId = Guid.NewGuid().ToString();
            var toListId = Guid.NewGuid().ToString();

            var task = new Domain.Task
            {
                Id = taskId
            };

            var fromList = new List
            {
                Id = fromListId,
                Tasks = new List<Domain.Task> {task}
            };

            var toList = new List
            {
                Id = fromListId,
                Tasks = new List<Domain.Task>()
            };

            var command = new MoveTaskCommand {FromListId = fromListId, ToListId = toListId, TaskId = taskId};
            var handler = new MoveTaskCommandHandler(repo.Object);

            repo.Setup(r => r.GetById(fromListId)).ReturnsAsync(fromList);
            repo.Setup(r => r.GetById(toListId)).ReturnsAsync(toList);

            await handler.Execute(command);

            Assert.AreEqual(0, fromList.Tasks.Count);
            Assert.AreEqual(1, toList.Tasks.Count);
            Assert.IsTrue(toList.Tasks.Contains(task));
        }

        [TestMethod]
        public async Task Execute_ThrowsResourceNotFoundExceptionOnNoMatchingTask()
        {
            var repo = new Mock<IListRepository>();
            var taskId = Guid.NewGuid().ToString();
            var fromListId = Guid.NewGuid().ToString();
            var toListId = Guid.NewGuid().ToString();

            var fromList = new List
            {
                Id = fromListId,
                Tasks = new List<Domain.Task>()
            };

            var toList = new List
            {
                Id = fromListId,
                Tasks = new List<Domain.Task>()
            };

            var command = new MoveTaskCommand {FromListId = fromListId, ToListId = toListId, TaskId = taskId};
            var handler = new MoveTaskCommandHandler(repo.Object);

            repo.Setup(r => r.GetById(fromListId)).ReturnsAsync(fromList);
            repo.Setup(r => r.GetById(toListId)).ReturnsAsync(toList);

            await Assert.ThrowsExceptionAsync<ResourceNotFoundException>(() => handler.Execute(command));
        }
    }
}