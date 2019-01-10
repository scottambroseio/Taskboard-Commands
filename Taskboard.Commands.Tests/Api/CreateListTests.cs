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
    public class CreateListTests
    {
        [TestMethod]
        public async Task ValidRequest_ReturnsCorrectResponse()
        {
            var handler = new Mock<ICommandHander<CreateListCommand, Uri>>();
            var container = new Container();
            var list = new ListDTO {Name = "list"};
            var location = new Uri("https://www.test.co.uk");

            handler.Setup(h => h.Execute(It.IsAny<CreateListCommand>()))
                .ReturnsAsync(Option.Some<Uri, CommandFailure>(location));
            container.RegisterInstance(handler.Object);
            CreateList.Container = container;

            var result = await CreateList.Run(list) as CreatedResult;

            Assert.IsNotNull(result);

            Assert.IsNull(result.Value);
            Assert.AreEqual(location, result.Location);
        }
    }
}