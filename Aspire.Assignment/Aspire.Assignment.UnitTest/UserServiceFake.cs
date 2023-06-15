using Assignment.Contracts.Data;
using Assignment.Contracts.Data.Entities;
using Assignment.Contracts.DTO;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Assignment.UnitTest
{
    public class UserServiceFake
    {
        public static Mock<IUnitOfWork> getUserRepository()
        {
            var mockRepo = new Mock<IUnitOfWork>();

            mockRepo.Setup(r => r.User.GetAll());
            return mockRepo;
        }


        public static Mock<IMapper> GetMapper()
        {
            var mockRepo = new Mock<IMapper>();
            return mockRepo;
        }

        public static Mock<IValidator<CreateUserDTO>> getValidator()
        {
            var mockRepo = new Mock<IValidator<CreateUserDTO>>();

            return mockRepo;
        }
        public static Mock<IPasswordHasher<User>> setPasswordHasher()
        {
            var mockRepo = new Mock<IPasswordHasher<User>>();

            return mockRepo;
        }

        public static Mock<IUnitOfWork> getUserbyName(string userName)
        {
            var mockRepo = new Mock<IUnitOfWork>();
           var d = mockRepo.Setup(r => r.User.GetByEmail(userName));
            return mockRepo;
        }
    }
}
