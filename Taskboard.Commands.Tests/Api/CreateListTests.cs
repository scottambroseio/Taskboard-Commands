using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            var handler = new Mock<ICommandHander<CreateListCommand, string>>();
            var container = new Container();
            var logger = new Mock<ILogger>().Object;
            var list = new ListDTO {Name = "list"};
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            handler.Setup(h => h.Execute(It.IsAny<CreateListCommand>()))
                .ReturnsAsync(Option.Some<string, OperationFailure>("location"));
            container.RegisterInstance(handler.Object);
            CreateList.Container = container;

            var result = await CreateList.Run(request, list, logger) as CreatedResult;

            Assert.IsNotNull(result);

            Assert.IsNull(result.Value);
            Assert.AreEqual("location", result.Location);
        }
    }
}