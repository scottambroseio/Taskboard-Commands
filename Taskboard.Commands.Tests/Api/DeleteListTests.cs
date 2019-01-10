using System;
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
            var logger = new Mock<ILogger>().Object;
            var id = Guid.NewGuid().ToString();
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            handler.Setup(h => h.Execute(It.IsAny<DeleteListCommand>()))
                .ReturnsAsync(Option.None<OperationFailure>());
            container.RegisterInstance(handler.Object);
            CreateList.Container = container;

            var result = await DeleteList.Run(request, id, logger) as NoContentResult;

            Assert.IsNotNull(result);
        }
    }
}