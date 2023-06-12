using Assignment.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Assignment.UnitTest
{
    public class AppControllerTest
    {

        private readonly AppController _controller;
        private readonly IMediator _mediator;
        private readonly ILogger<AppController> _logger;

        public AppControllerTest() {

            _controller = new AppController(_mediator, _logger);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.Get();
            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }
    }
}
