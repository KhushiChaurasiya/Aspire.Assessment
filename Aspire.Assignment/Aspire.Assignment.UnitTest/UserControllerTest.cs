using Assignment.Contracts.Data;
using Assignment.Contracts.Data.Entities;
using Assignment.Contracts.DTO;
using Assignment.Controllers;
using Assignment.Core.Handlers.Queries;
using Assignment.Core.Mapper;
using Assignment.Providers.Handlers.Commands;
using Assignment.Providers.Handlers.Queries;
using AutoMapper;
using Azure.Core;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Aspire.Assignment.UnitTest
{
    public class UserControllerTest
    {

        private Mock<IMediator> Mediator;
        private Mock<IUnitOfWork> repository;
        private Mock<IMapper> mapper;
        private Mock<IConfiguration> configuration;
        private Mock<ILogger<UserController>> logger;
        private Mock<IValidator<CreateUserDTO>> validator;
        private Mock<GetUserByUserNameQuery> getUserByUserNameQuery;
        private Mock<GetUserByUserNameQueryHandler> getUserByUserNameQueryHandler;
        private Mock<IPasswordHasher<User>> _passwordHasher;

        public UserControllerTest()
        {
            Mediator = new Mock<IMediator>();
            repository = new Mock<IUnitOfWork>();
            mapper = new Mock<IMapper>();
            logger = new Mock<ILogger<UserController>>();
            configuration = new Mock<IConfiguration>();
            validator = new Mock<IValidator<CreateUserDTO>>();
            getUserByUserNameQuery = new Mock<GetUserByUserNameQuery>();
            getUserByUserNameQueryHandler = new Mock<GetUserByUserNameQueryHandler>();
           _passwordHasher = new Mock<IPasswordHasher<User>>();
        }

        //passed
        [Fact]
        public async void Get_WhenCalled_ReturnsOkResult()
        {

            GetAllUserQuery command = new GetAllUserQuery();
            var mokeUserRepository = UserServiceFake.getUserRepository();
            var mokeMapper = UserServiceFake.GetMapper();
            GetAllUserQueryHandler handler = new GetAllUserQueryHandler(mokeUserRepository.Object, mokeMapper.Object);
            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.IsType<UserDTO[]>(result);
        }

        [Fact]
        public async void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            CreateUserDTO model = new CreateUserDTO()
            {
                Id = 1,
                Firstname = "test",
                Lastname = "test",
                Username = "test",
                Email = "Khushboo@gmail.com",
                Password = "test",
                Address = "test",
                Role = "User",
                Provider = "test",
                IdToken = "test"
            };

            validator.Setup(x => x.Validate(It.IsAny<CreateUserDTO>()).IsValid).Returns(true);     
            _passwordHasher.Setup(x => x.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns(It.IsAny<string>());
            repository.Setup(x => x.User.Add(It.IsAny<User>()));


            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();


            CreateUserCommandHandler handler = new CreateUserCommandHandler(repository.Object, validator.Object, _passwordHasher.Object);
            var result = await handler.Handle(new CreateUserCommand(model), CancellationToken.None);
            Assert.IsType<Int32>(result);

        }

        // passed
        [Fact]
        public async void GetByUserName_ExistingGuidPassed_ReturnsRightItem()
        {
            var userEmail = "Khushboo@gmail.com";
            var user = new User()
            {
                Id = 1,
                Firstname ="test",
                Lastname="test",
                Username="test",
                Email = "Khushboo@gmail.com",
                Password ="test",
                Address ="test",
                Role ="User",
                Provider ="test",
                IdToken = "test"

           };

            repository.Setup(x => x.User.GetByEmail(It.IsAny<string>())).Returns(user);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();
            GetUserByUserNameQueryHandler handler = new GetUserByUserNameQueryHandler(repository.Object, mapper);
            var result = await handler.Handle(new GetUserByUserNameQuery(userEmail), CancellationToken.None);
            Assert.IsType<UserDTO>(result);

        }

        //passed
        [Fact]
        public async void GetuserReport_ExistingGuidPassed_ReturnsRightItem()
        {
            GetAllUserReportQuery user = new GetAllUserReportQuery();
            var mokeUserRepository = UserServiceFake.getUserRepository();
            var mokeMapper = UserServiceFake.GetMapper();
            GetAllUserReportQueryHandler _handler = new GetAllUserReportQueryHandler(mokeUserRepository.Object, mokeMapper.Object);
            var result = await _handler.Handle(user, CancellationToken.None);
            Assert.IsType<Int32>(result);

        }

    }
}
