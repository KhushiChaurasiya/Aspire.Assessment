using Assignment.Contracts.Data.Repositories;
using Assignment.Contracts.Data;
using Assignment.Contracts.DTO;
using Assignment.Controllers;
using Assignment.Core.Data.Repositories;
using Assignment.Providers.Handlers.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Aspire.Assignment.UnitTest
{
    public class AppControllerTest
    {
        private Mock<IMediator> Mediator;
        private Mock<IUnitOfWork> repository;
        private Mock<IMapper> mapper;
        private Mock<ILogger<GetAllAppQueryHandler>> logger;

        public AppControllerTest()
        {
            Mediator = new Mock<IMediator>();
            repository = new Mock<IUnitOfWork>();
            mapper = new Mock<IMapper>();   
            logger = new Mock<ILogger<GetAllAppQueryHandler>>();
        }


        [Fact]
        public async void Get_WhenCalled_ReturnsOkResult()
        {
            var App = new GetAllAppQuery();
            var mokeAppRepository = AppServiceFake.GetAppRepository();
            var mokeMapper = AppServiceFake.GetMapper();
            var _handler = new GetAllAppQueryHandler(mokeAppRepository.Object, mokeMapper.Object,logger.Object);
            var result = await _handler.Handle(App, CancellationToken.None);
        }

        [Fact]
        public async void GetById_WhenCalled_ReturnsOkResult()
        {
            int id = 2;
            var App = new GetAppByIdQuery(id);
            var mokeAppRepository = AppServiceFake.GetAppRepository();
            var mokeMapper = AppServiceFake.GetMapper();
            var _handler = new GetAppByIdQueryHandler(mokeAppRepository.Object, mokeMapper.Object);
            var result = await _handler.Handle(App, CancellationToken.None);
        }


    }
}
