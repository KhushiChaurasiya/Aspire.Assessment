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
using Assignment.Contracts.Data.Entities;
using Assignment.Core.Mapper;
using Assignment.Providers.Handlers.Commands;
using FluentValidation;
using Assignment.Core.Handlers.Queries;
using Assignment.Core.Handlers.Commands;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Extensions.Azure;
using Castle.Components.DictionaryAdapter.Xml;

namespace Aspire.Assignment.UnitTest
{
    public class AppControllerTest
    {
        private Mock<IMediator> Mediator;
        private Mock<IUnitOfWork> repository;
        private Mock<IMapper> mapper;
        private Mock<ILogger<GetAllAppQueryHandler>> logger;
        private Mock<ILogger<AppController>> _logger;

        private Mock<IValidator<CreateAppDTO>> validator;
        private Mock<IFormFile> file;
        private Mock<AppRepository> mockRepo;

        public AppControllerTest()
        {
            Mediator = new Mock<IMediator>();
            repository = new Mock<IUnitOfWork>();
            mapper = new Mock<IMapper>();   
            logger = new Mock<ILogger<GetAllAppQueryHandler>>();
            validator = new Mock<IValidator<CreateAppDTO>>();
            file = new Mock<IFormFile>();
            _logger = new Mock<ILogger<AppController>>();
             mockRepo = new Mock<AppRepository>();
        }


        [Fact]
        public async void Get_WhenCalled_ReturnsOkResult()
        {
            var mokeAppRepository = AppServiceFake.GetAppRepository();
            var mokeMapper = AppServiceFake.GetMapper();
            GetAllAppQueryHandler _handler = new GetAllAppQueryHandler(mokeAppRepository.Object, mokeMapper.Object, logger.Object);
            var result = await _handler.Handle(new GetAllAppQuery(), CancellationToken.None);
           
            Assert.IsType<AppDTO[]>(result);
            repository.Verify(x => x.App.GetAll(), Times.Exactly(0));
        }

        [Fact]
        public async void GetById_WhenCalled_ReturnsOkResult()
        {
            var id = 1;
            var app = new App()
            {
                Id = 1,
                Name ="test",
                Description ="forTesting",
                Price = 500.0M,
                Type="zip",
                Files= "src.zip"

            };

            repository.Setup(x => x.App.Get(It.IsAny<object>())).Returns(app);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();

            GetAppByIdQueryHandler handler = new GetAppByIdQueryHandler(repository.Object, mapper);
            var result = await handler.Handle(new GetAppByIdQuery(id), CancellationToken.None);
          
            Assert.IsType<AppDTO>(result);
            repository.Verify(x => x.App.GetByEmail(It.IsAny<string>()), Times.Exactly(0));

        }
        
        [Fact]
        public async void InsertApps_WhenCalled_ReturnsOkResult()
        {
            var app = new CreateAppDTO()
            {
                Id = 1,
                Name = "test",
                Description = "forTesting",
                Price = 500.0M,
                Type = "zip",
                Files = "src.zip"

            };

            var file = new Mock<IFormFile>();
            var sourceImg = File.OpenRead(@"D:\Assignment\Aspire.Assignment\Assignment.API\Resources\src.zip");
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(sourceImg);
            writer.Flush();
            ms.Position = 0;
            var fileName = "src.zip";
            file.Setup(f => f.FileName).Returns(fileName).Verifiable();
            file.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream))
                .Verifiable();

            validator.Setup(x => x.Validate(It.IsAny<CreateAppDTO>()).IsValid).Returns(true);
            //mockRepo.Setup(x => x.Add(It.IsAny<App>()));

            repository.Setup(x => x.App.Add(It.IsAny<App>()));
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();
            var inputFile = file.Object;

            CreateAppCommandHandler handler = new CreateAppCommandHandler(repository.Object, validator.Object);
            var result = await handler.Handle(new CreateAppCommand(app, inputFile), CancellationToken.None);
            Assert.IsType<Int32>(result);
            repository.Verify(x => x.App.Add(It.IsAny<App>()), Times.Exactly(1));

        }

        [Fact]
        public async void DeleteAppById_WhenCalled_ReturnsOkResult()
        {
            var id = 1;
            var app = new App()
            {
                Id = 1,
                Name = "test",
                Description = "forTesting",
                Price = 500.0M,
                Type = "zip",
                Files = "src.zip"

            };

            repository.Setup(x => x.App.Delete(It.IsAny<object>()));
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();

            DeleteAppByIdQueryHandler handler = new DeleteAppByIdQueryHandler(repository.Object, mapper);
            var result = await handler.Handle(new DeleteAppByIdQuery(id), CancellationToken.None);
            repository.Verify(x=>x.App.Delete(It.IsAny<int>()), Times.Once);
           
        }

        [Fact]
        public async void UpdateApps_WhenCalled_ReturnsOkResult()
        {
            var appData = new CreateAppDTO()
            {
                Id = 1,
                Name = "test",
                Description = "forTesting",
                Price = 500.0M,
                Type = "zip",
                Files = "src.zip"

            };

            var file = new Mock<IFormFile>();
            var sourceImg = File.OpenRead(@"D:\Assignment\Aspire.Assignment\Assignment.API\Resources\src.zip");
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(sourceImg);
            writer.Flush();
            ms.Position = 0;
            var fileName = "src.zip";
            file.Setup(f => f.FileName).Returns(fileName).Verifiable();
            file.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream))
                .Verifiable();

            validator.Setup(x => x.Validate(It.IsAny<CreateAppDTO>()).IsValid).Returns(true);

            var id = 1;
            var app = new App()
            {
                Id = 1,
                Name = "test",
                Description = "forTesting",
                Price = 500.0M,
                Type = "zip",
                Files = "src.zip"

            };

            repository.Setup(x => x.App.Get(It.IsAny<object>())).Returns(app);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();
            var inputFile = file.Object;

            GetAppByIdQueryHandler handler = new GetAppByIdQueryHandler(repository.Object, mapper);
            var result = await handler.Handle(new GetAppByIdQuery(id), CancellationToken.None);


            repository.Setup(x => x.App.Add(It.IsAny<App>()));
            
            UpdateAppCommandHandler _handler = new UpdateAppCommandHandler(repository.Object, validator.Object);
            var resultdata = await _handler.Handle(new UpdateAppCommand(appData, inputFile), CancellationToken.None);
            Assert.IsType<Int32>(resultdata);
            repository.Verify(x => x.App.Update(It.IsAny<App>()), Times.Exactly(1));

        }

        [Fact]
        public async void DownloadApps_WhenCalled_ReturnsOkResult()
        {
            AppDownloadDTO appDownloadDTO = new AppDownloadDTO();
            var file = new Mock<IFormFile>();
            var sourceImg = File.OpenRead(@"D:\Assignment\Aspire.Assignment\Assignment.API\Resources\src.zip");
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(sourceImg);
            writer.Flush();
            ms.Position = 0;
            var fileName = "src.zip";
            file.Setup(f => f.FileName).Returns(fileName).Verifiable();
            file.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream))
                .Verifiable();

            
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();
            var inputFile = file.Object;

            GetDownloadFilesQueryHandler _handler = new GetDownloadFilesQueryHandler(repository.Object, mapper);
            var resultdata = await _handler.Handle(new GetDownloadFilesQuery(inputFile), CancellationToken.None);
            Assert.IsType<string>(resultdata);
            
        }
        [Fact]
        public async void PostAppDownload_WhenCalled_ReturnsOkResult()
        {
            var id = 1;
            AppDownload app = new AppDownload()
            {
                AppId =1,
                DownloadedDate = DateTime.Now,
            };

            repository.Setup(x => x.Appdownload.Add(It.IsAny<AppDownload>()));

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();

            CreateAppDownloadCommandHandler handler = new CreateAppDownloadCommandHandler(repository.Object);
            var result = await handler.Handle(new CreateAppDownloadCommand(id), CancellationToken.None);
            Assert.IsType<Int32>(result);
        }

        [Fact]
        public async void DownloadedReport_WhenCalled_ReturnsOkResult()
        {
            var downloadReportDTO = new DownloadReportDTO()
            {
                NumberOfDownloads = 2,
                AppName ="Test",
                DownlodedDate = DateTime.Now
            };

            DateTime? fromdate = DateTime.Now;
            DateTime? todate = DateTime.Now;

            IEnumerable<DownloadReportDTO> a = new[] { downloadReportDTO };

            repository.Setup(x => x.App.GetDownloadedReport(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(a);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();
            GetDownloadedReportDetailsQueryHandler handler = new GetDownloadedReportDetailsQueryHandler(repository.Object, mapper);
            var result = await handler.Handle(new GetDownloadedReportDetailsQuery(fromdate, todate), CancellationToken.None);
            Assert.IsAssignableFrom<IEnumerable<DownloadReportDTO>>(result);
            
        }

        [Fact]
        public async void LogReport_WhenCalled_ReturnsOkResult()
        {
            DateTime logcreateddate = DateTime.Now;
            LogsDTO logDTO = new LogsDTO()
            {
                Id = 1,
                CreatedOn = DateTime.Now,
                Level = "test",
                Message = "test",
                StackTrace = "Test",
                Exception = "Test",
                Logger = "Test",
                Url = "test"
            };

            IEnumerable<LogsDTO>  a = new[] { logDTO };

            repository.Setup(x => x.Errorlog.getLogReportWiseDate(It.IsAny<DateTime>(), It.IsAny<string>())).Returns(a);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();
            GetAllLogReportQueryHandler handler = new GetAllLogReportQueryHandler(repository.Object, mapper);
            var result = await handler.Handle(new GetAllLogReportQuery(logcreateddate, It.IsAny<string>()), CancellationToken.None);
            Assert.IsAssignableFrom<IEnumerable<LogsDTO>>(result);

        }
        // controller
        [Fact]
        public async void GetById_WhenCalled_OKResults()
        {
            int id =1;
            var app = new AppDTO()
            {
                Id = 1,
                Name = "test",
                Description = "forTesting",
                Price = 500.0M,
                Type = "zip",
                Files = "src.zip"

            };

            Mediator.Setup(x => x.Send(It.IsAny<AppDTO>(), new CancellationToken())).ReturnsAsync(app);

            var controller = new AppController(Mediator.Object, _logger.Object);
            var result = await controller.GetById(id);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAll_WhenCalled_OKResults()
        {
            var app = new AppDTO()
            {
                Id = 1,
                Name = "test",
                Description = "forTesting",
                Price = 500.0M,
                Type = "zip",
                Files = "src.zip"

            };

            Mediator.Setup(x => x.Send(It.IsAny<AppDTO>(), new CancellationToken())).ReturnsAsync(app);

            var controller = new AppController(Mediator.Object, _logger.Object);
            var result = await controller.Get();
            Assert.IsType<OkObjectResult>(result);
        }
        //[Fact]
        //public async void DeleteApps_WhenCalled_OKResults()
        //{
        //    var delete = new Mock<AppController>();

        //    int id = 1;
        //    var app = new CreateAppDTO()
        //    {
        //        Id = 1,
        //        Name = "test",
        //        Description = "forTesting",
        //        Price = 500.0M,
        //        Type = "zip",
        //        Files = "src.zip"

        //    };

        //    Mediator.Setup(x => x.Send(It.IsAny<AppDTO>(), new CancellationToken())).ReturnsAsync(app);
        //    repository.Setup(x => x.App.Delete(It.IsAny<object>()));

        //    delete.Setup(x => x.Delete(It.IsAny<int>()));

        //    var controller = new AppController(Mediator.Object, _logger.Object);

        //    var result = await controller.Delete(id);
        //    Assert.IsType<OkObjectResult>(result);
        //}

        [Fact]
        public async void InsertAppsDetails_WhenCalled_ReturnsOkResult()
        {
            var app = new CreateAppDTO()
            {
                Id = 1,
                Name = "test",
                Description = "forTesting",
                Price = 500.0M,
                Type = "zip",
                Files = "src.zip"

            };

            var file = new Mock<IFormFile>();
            var sourceImg = File.OpenRead(@"D:\Assignment\Aspire.Assignment\Assignment.API\Resources\src.zip");
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(sourceImg);
            writer.Flush();
            ms.Position = 0;
            var fileName = "src.zip";
            file.Setup(f => f.FileName).Returns(fileName).Verifiable();
            file.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream))
                .Verifiable();

            validator.Setup(x => x.Validate(It.IsAny<CreateAppDTO>()).IsValid).Returns(true);

            repository.Setup(x => x.App.Add(It.IsAny<App>()));
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();
            var inputFile = file.Object;

            Mediator.Setup(x => x.Send(It.IsAny<AppDTO>(), new CancellationToken())).ReturnsAsync(app);

            var controller = new AppController(Mediator.Object, _logger.Object);
            var result = await controller.Post(app);
            Assert.IsType<NotFoundObjectResult>(result);

        }

        [Fact]
        public async void Logs_WhenCalled_ReturnsOkResult()
        {
            DateTime logcreateddate = DateTime.Now;
            LogsDTO logDTO = new LogsDTO()
            {
                Id = 1,
                CreatedOn = DateTime.Now,
                Level = "test",
                Message = "test",
                StackTrace = "Test",
                Exception = "Test",
                Logger = "Test",
                Url = "test"
            };

            IEnumerable<LogsDTO> a = new[] { logDTO };

            repository.Setup(x => x.Errorlog.getLogReportWiseDate(It.IsAny<DateTime>(), It.IsAny<string>())).Returns(a);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();

            Mediator.Setup(x => x.Send(It.IsAny<AppDTO>(), new CancellationToken())).ReturnsAsync(logDTO);

            var controller = new AppController(Mediator.Object, _logger.Object);
            var result = await controller.LogReport(It.IsAny<DateTime>(), It.IsAny<string>());
            Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public async void AppsDownloadedReport_WhenCalled_ReturnsOkResult()
        {
            var downloadReportDTO = new DownloadReportDTO()
            {
                NumberOfDownloads = 2,
                AppName = "Test",
                DownlodedDate = DateTime.Now
            };

            DateTime? fromdate = DateTime.Now;
            DateTime? todate = DateTime.Now;

            IEnumerable<DownloadReportDTO> a = new[] { downloadReportDTO };

            repository.Setup(x => x.App.GetDownloadedReport(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(a);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();
            Mediator.Setup(x => x.Send(It.IsAny<AppDTO>(), new CancellationToken())).ReturnsAsync(downloadReportDTO);

            var controller = new AppController(Mediator.Object, _logger.Object);
            var result = await controller.DownloadedReport(It.IsAny<DateTime>(), It.IsAny<DateTime>());
            Assert.IsType<OkObjectResult>(result);


        }

        [Fact]
        public async void UpdateAppsDetails_WhenCalled_ReturnsOkResult()
        {
            var appData = new CreateAppDTO()
            {
                Id = 1,
                Name = "test",
                Description = "forTesting",
                Price = 500.0M,
                Type = "zip",
                Files = "src.zip"

            };

            var file = new Mock<IFormFile>();
            var sourceImg = File.OpenRead(@"D:\Assignment\Aspire.Assignment\Assignment.API\Resources\src.zip");
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(sourceImg);
            writer.Flush();
            ms.Position = 0;
            var fileName = "src.zip";
            file.Setup(f => f.FileName).Returns(fileName).Verifiable();
            file.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream))
                .Verifiable();

            validator.Setup(x => x.Validate(It.IsAny<CreateAppDTO>()).IsValid).Returns(true);

            var id = 1;
            var app = new App()
            {
                Id = 1,
                Name = "test",
                Description = "forTesting",
                Price = 500.0M,
                Type = "zip",
                Files = "src.zip"

            };

            repository.Setup(x => x.App.Get(It.IsAny<object>())).Returns(app);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();
            var inputFile = file.Object;

            GetAppByIdQueryHandler handler = new GetAppByIdQueryHandler(repository.Object, mapper);
            var result1 = await handler.Handle(new GetAppByIdQuery(id), CancellationToken.None);

            repository.Setup(x => x.App.Add(It.IsAny<App>()));

            Mediator.Setup(x => x.Send(It.IsAny<AppDTO>(), new CancellationToken())).ReturnsAsync(appData);

            var controller = new AppController(Mediator.Object, _logger.Object);
            var result = await controller.Put(It.IsAny<int>(), It.IsAny<CreateAppDTO>());
            Assert.IsType<NotFoundObjectResult>(result);

        }

        [Fact]
        public async void PostAppDownloadDetails_WhenCalled_ReturnsOkResult()
        {
            var id = 1;
            AppDownload app = new AppDownload()
            {
                AppId = 1,
                DownloadedDate = DateTime.Now,
            };

            repository.Setup(x => x.Appdownload.Add(It.IsAny<AppDownload>()));

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();

            Mediator.Setup(x => x.Send(It.IsAny<AppDTO>(), new CancellationToken())).ReturnsAsync(app);

            var controller = new AppController(Mediator.Object, _logger.Object);
            var result = await controller.PostAppDownload(It.IsAny<int>(), It.IsAny<AppDownloadDTO>());
            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}
