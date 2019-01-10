using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Optional;
using SimpleInjector;
using Taskboard.Commands.Api;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Enums;
using Taskboard.Commands.Handlers;

namespace Taskboard.Commands.Tests.Api
{
    [TestClass]
    public class DeleteListTests
    {
        [TestMethod]
        public async Task ValidRequest_ReturnsCorrectResponse()
        {
            var handler = new Mock<ICommandHander<DeleteListCommand>>();
            var container = new Container();
            var id = Guid.NewGuid().ToString();

            handler.Setup(h => h.Execute(It.IsAny<DeleteListCommand>()))
                .ReturnsAsync(Option.None<CommandFailure>());
            container.RegisterInstance(handler.Object);
            DeleteList.Container = container;

            var result = await DeleteList.Run(id) as NoContentResult;

            Assert.IsNotNull(result);
        }
    }
}