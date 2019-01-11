using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Optional;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Domain;
using Taskboard.Commands.Enums;
using Taskboard.Commands.Handlers;
using Taskboard.Commands.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Taskboard.Commands.Tests.Handlers
{
    [TestClass]
    public class CreateTaskCommandHandlerTests
    {
        [TestMethod]
        public async Task Execute_ReturnsNotFoundWhenListDoesNotExist()
        {
            var repo = new Mock<IListRepository>();
            var command = new CreateTaskCommand
            {
                ListId = Guid.NewGuid().ToString(),
                Description = "description",
                Name = "name"
            };
            var handler = new CreateTaskCommandHandler(repo.Object);

            repo.Setup(r => r.GetById(It.IsAny<string>()))
                .ReturnsAsync(Option.None<List, CosmosFailure>(CosmosFailure.NotFound));

            var result = await handler.Execute(command);

            result.Match(
                failure => Assert.AreEqual(CommandFailure.NotFound, failure),
                () => Assert.Fail()
            );
        }

        [TestMethod]
        public async Task Execute_ReturnsCorrectResultOnSuccessAndCreatesTask()
        {
            var repo = new Mock<IListRepository>();
            var list = new List();
            var command = new CreateTaskCommand
            {
                ListId = Guid.NewGuid().ToString(),
                Description = "description",
                Name = "name"
            };
            var handler = new CreateTaskCommandHandler(repo.Object);

            repo.Setup(r => r.GetById(It.IsAny<string>()))
                .ReturnsAsync(Option.Some<List, CosmosFailure>(list));

            var result = await handler.Execute(command);

            result.MatchSome(failure => Assert.Fail());

            repo.Verify(r => r.Replace(list));

            Assert.AreEqual(1, list.Tasks.Count);
        }
    }
}