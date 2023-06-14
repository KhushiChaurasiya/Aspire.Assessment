using Assignment.Contracts.Data;
using Assignment.Contracts.DTO;
using Assignment.Controllers;
using Assignment.Providers.Handlers.Queries;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Assignment.UnitTest
{
    public class AppServiceFake
    {
        public static Mock<IUnitOfWork> GetAppRepository()
        {
            var mockRepo = new Mock<IUnitOfWork>();

            mockRepo.Setup(r => r.App.GetAll());
            return mockRepo;
        }
        public static Mock<IMapper> GetMapper()
        {
            var mockRepo = new Mock<IMapper>();

            return mockRepo;
        }
        //public static Mock<ILogger<GetAllAppQueryHandler> logger> GetMapper()
        //{
        //    var mockRepo = new Mock<IMapper>();

        //    mockRepo.Setup(r => r);
        //    return mockRepo;
        //}
    }
}
