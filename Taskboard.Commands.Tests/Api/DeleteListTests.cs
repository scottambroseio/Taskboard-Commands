using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Taskboard.Commands.Api;

namespace Taskboard.Commands.Tests.Api
{
    [TestClass]
    public class DeleteListTests
    {
        [TestMethod]
        public async Task ValidRequest_ReturnsCorrectResponse()
        {
            var logger = new Mock<ILogger>().Object;
            var id = Guid.NewGuid().ToString();
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            var result = await DeleteList.Run(request, id, logger) as NoContentResult;

            Assert.IsNotNull(result);
        }
    }
}