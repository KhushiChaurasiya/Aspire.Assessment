using Assignment.Contracts.Data;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Assignment.UnitTest
{
    public class AuthServiceFake
    {
        public static Mock<IUnitOfWork> getAuthRepository()
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
    }
}
