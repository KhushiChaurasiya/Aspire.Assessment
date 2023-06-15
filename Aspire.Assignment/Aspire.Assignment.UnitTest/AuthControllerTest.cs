using Assignment.Contracts.Data.Entities;
using Assignment.Contracts.Data;
using Assignment.Contracts.DTO;
using Assignment.Controllers;
using Assignment.Providers.Handlers.Queries;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Assignment.Core.Data.Repositories;
using NLog;
using System.ComponentModel.DataAnnotations;
using Assignment.Contracts.Data.Repositories;
using Assignment.Core.Mapper;
using Assignment.Providers.Handlers.Commands;

namespace Aspire.Assignment.UnitTest
{
    public class AuthControllerTest
    {
        private  Mock<IMediator> _mediator;
        private Mock<IConfiguration> _configuration;
        private Mock<IPasswordHasher<User>> _passwordHasher;
        private Mock<AppSettingsDTO> _applicationSettings;
        private Mock<ILogger<AuthController>> _logger;
        private Mock<IUnitOfWork> repository;

        public AuthControllerTest()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<AuthController>>();
            _configuration = new Mock<IConfiguration>();
            _applicationSettings = new Mock<AppSettingsDTO>();
            _passwordHasher = new Mock<IPasswordHasher<User>>();
            repository = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async void Get_WhenCalled_ReturnsOkResult()
        {

            GetAllUserQuery command = new GetAllUserQuery();
            var mokeAuthRepository = AuthServiceFake.getAuthRepository();
            var mokeMapper = AuthServiceFake.GetMapper();
            GetAllUserQueryHandler handler = new GetAllUserQueryHandler(mokeAuthRepository.Object, mokeMapper.Object);
            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.IsType<UserDTO[]>(result);
        }

        //[Fact]
        //public async void AddAuthUser_InvalidObjectPassed_ReturnsBadRequest()
        //{
        //    // Arrange
        //    string password = "test";
        //    string username = "test";

        //    User model = new User()
        //    {
        //        Id = 1,
        //        Firstname = "test",
        //        Lastname = "test",
        //        Username = "test",
        //        Email = "Khushboo@gmail.com",
        //        Password = "test",
        //        Address = "test",
        //        Role = "User",
        //        Provider = "test",
        //        IdToken = "test"
        //    };

        //    IEnumerable<User> a = new[] { model };
        //    _passwordHasher.Setup(x => x.VerifyHashedPassword(model,It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Success);
        //    repository.Setup(x => x.User.GetAll()).Returns(a);


        //    var mockMapper = new MapperConfiguration(cfg =>
        //    {
        //        cfg.AddProfile(new AutoMapperProfile());
        //    });

        //    var mapper = mockMapper.CreateMapper();


        //    SignInUserByUserNameQueryHandler handler = new SignInUserByUserNameQueryHandler(repository.Object,mapper, _passwordHasher.Object, _configuration.Object);
        //    var result = await handler.Handle(new SignInUserByUserNameQuery(model), CancellationToken.None);
        //    Assert.IsType<Int32>(result);

        //}

        // passed
        [Fact]
        public async void GetById_ExistingGuidPassed_ReturnsRightItem()
        {
            var username = "Khushboo@gmail.com";
            var user = new User()
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

            repository.Setup(x => x.User.GetByEmail(It.IsAny<string>())).Returns(user);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();
            GetUserByUserNameQueryHandler handler = new GetUserByUserNameQueryHandler(repository.Object, mapper);
            var result = await handler.Handle(new GetUserByUserNameQuery(username), CancellationToken.None);
            Assert.IsType<UserDTO>(result);

        }
    }

    
}
