using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Optional;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Domain;
using Taskboard.Commands.Enums;
using Taskboard.Commands.Handlers;
using Taskboard.Commands.Repositories;

namespace Taskboard.Commands.Tests.Handlers
{
    [TestClass]
    public class CreateListCommandHandlerTests
    {
        [TestMethod]
        public async Task Execute_ReturnsCorrectUriOnSuccess()
        {
            Environment.SetEnvironmentVariable("LIST_RESOURCE_URI", "http://localhost:7071/api/list/{0}");

            var repo = new Mock<IListRepository>();
            var id = Guid.NewGuid().ToString();
            var command = new CreateListCommand {Name = "test"};
            var handler = new CreateListCommandHandler(repo.Object);

            repo.Setup(r => r.Create(It.IsAny<List>())).ReturnsAsync(Option.Some<string, CosmosFailure>(id));

            var result = await handler.Execute(command);

            result.Match(
                uri => Assert.AreEqual(new Uri($"http://localhost:7071/api/list/{id}"), uri),
                failure => Assert.Fail()
            );
        }

        [TestMethod]
        public async Task Execute_ReturnsErrorOnFailure()
        {
            var repo = new Mock<IListRepository>();
            var command = new CreateListCommand {Name = "test"};
            var handler = new CreateListCommandHandler(repo.Object);

            repo.Setup(r => r.Create(It.IsAny<List>()))
                .ReturnsAsync(Option.None<string, CosmosFailure>(CosmosFailure.Error));

            var result = await handler.Execute(command);

            result.MatchSome(
                uri => Assert.Fail()
            );
        }
    }
}