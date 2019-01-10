using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Taskboard.Commands.Api;
using Taskboard.Commands.DTO;

namespace Taskboard.Commands.Tests.Api
{
    [TestClass]
    public class CreateListTests
    {
        [TestMethod]
        public async Task ValidRequest_ReturnsCorrectResponse()
        {
            var logger = new Mock<ILogger>().Object;
            var list = new ListDTO {Name = "list"};
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            var result = await CreateList.Run(request, list, logger) as CreatedResult;

            Assert.IsNotNull(result);

            Assert.IsNull(result.Value);
            Assert.AreEqual("location", result.Location);
        }
    }
}