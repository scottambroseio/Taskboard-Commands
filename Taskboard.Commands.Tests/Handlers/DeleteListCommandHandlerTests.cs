using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Optional;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Enums;
using Taskboard.Commands.Handlers;
using Taskboard.Commands.Repositories;

namespace Taskboard.Commands.Tests.Handlers
{
    [TestClass]
    public class DeleteListCommandHandlerTests
    {
        [TestMethod]
        public async Task Execute_ReturnsNoneOnSuccess()
        {
            var repo = new Mock<IListRepository>();
            var id = Guid.NewGuid().ToString();
            var command = new DeleteListCommand {Id = id};
            var handler = new DeleteListCommandHandler(repo.Object);

            repo.Setup(r => r.Delete(It.IsAny<string>())).ReturnsAsync(Option.None<CosmosFailure>());

            var result = await handler.Execute(command);

            result.MatchSome(failure => Assert.Fail());
        }

        [TestMethod]
        public async Task Execute_ReturnsErrorOnFailure()
        {
            var repo = new Mock<IListRepository>();
            var id = Guid.NewGuid().ToString();
            var command = new DeleteListCommand {Id = id};
            var handler = new DeleteListCommandHandler(repo.Object);

            repo.Setup(r => r.Delete(It.IsAny<string>())).ReturnsAsync(Option.Some(CosmosFailure.Error));

            var result = await handler.Execute(command);

            result.MatchNone(
                () => Assert.Fail()
            );
        }
    }
}