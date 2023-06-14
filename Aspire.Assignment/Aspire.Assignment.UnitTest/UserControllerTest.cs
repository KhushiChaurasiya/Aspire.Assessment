using Assignment.Contracts.Data;
using Assignment.Contracts.Data.Entities;
using Assignment.Contracts.DTO;
using Assignment.Core.Handlers.Queries;
using Assignment.Providers.Handlers.Commands;
using Assignment.Providers.Handlers.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Assignment.UnitTest
{
    public class UserControllerTest
    {

        private Mock<IMediator> Mediator;
        private Mock<IUnitOfWork> repository;
        private Mock<IMapper> mapper;
        private Mock<ILogger<GetAllAppQueryHandler>> logger;

        public UserControllerTest()
        {
            Mediator = new Mock<IMediator>();
            repository = new Mock<IUnitOfWork>();
            mapper = new Mock<IMapper>();
            logger = new Mock<ILogger<GetAllAppQueryHandler>>();
        }


        [Fact]
        public async void Get_WhenCalled_ReturnsOkResult()
        {
            var user = new GetAllUserQuery();
            var mokeUserRepository = UserServiceFake.getUserRepository();
            var mokeMapper = UserServiceFake.GetMapper();
            var _handler = new GetAllUserQueryHandler(mokeUserRepository.Object, mokeMapper.Object);
            var result = await _handler.Handle(user, CancellationToken.None);
        }

        [Fact]
        public async void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var model = new CreateUserDTO()
            {
                Firstname = "test",
                Lastname = "Test",
                Username = "test",
                Email = "test@gmail.com",
                Password ="test@1996",
                Address="gujarat",
                Role="user"
    };
            var UserData = new CreateUserCommand(model);
            var mokeUserRepository = UserServiceFake.getUserRepository();
            var mokeValidator = UserServiceFake.getValidator();
            var mokePasswordHasher = UserServiceFake.setPasswordHasher();
            var _handler = new CreateUserCommandHandler(mokeUserRepository.Object, mokeValidator.Object, mokePasswordHasher.Object);
            var result = await _handler.Handle(UserData, CancellationToken.None);
        }

        [Fact]
        public async void GetByUserName_ExistingGuidPassed_ReturnsRightItem()
        {

            var userName = "Khushboo@gmail.com";

            var user = new GetUserByUserNameQuery(userName);
            var mokeUserRepository = UserServiceFake.getUserbyName(userName);
            var mokeMapper = UserServiceFake.GetMapper();
            var _handler = new GetUserByUserNameQueryHandler(mokeUserRepository.Object, mokeMapper.Object);
            var result = await _handler.Handle(user, CancellationToken.None);

        }

        [Fact]
        public async void GetuserReport_ExistingGuidPassed_ReturnsRightItem()
        {
            var user = new GetAllUserReportQuery();
            var mokeUserRepository = UserServiceFake.getUserRepository();
            var mokeMapper = UserServiceFake.GetMapper();
            var _handler = new GetAllUserReportQueryHandler(mokeUserRepository.Object, mokeMapper.Object);
            var result = await _handler.Handle(user, CancellationToken.None);

        }

    }
}
