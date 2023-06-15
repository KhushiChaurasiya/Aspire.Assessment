using Assignment.Contracts.Data.Entities;
using Assignment.Contracts.Data;
using Assignment.Contracts.DTO;
using Assignment.Core.Exceptions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Assignment.Core.Handlers.Commands
{
    public class CreateAppDownloadCommand : IRequest<int>
    {
        public int appId { get; }
        public CreateAppDownloadCommand(int Id)
        {
            this.appId = Id;
        }
    }

    public class CreateAppDownloadCommandHandler : IRequestHandler<CreateAppDownloadCommand, int>
    {
        private readonly IUnitOfWork _repository;

        public CreateAppDownloadCommandHandler(IUnitOfWork repository)
        {
            _repository = repository;

        }

        public async Task<int> Handle(CreateAppDownloadCommand request, CancellationToken cancellationToken)
        {
            var entity = new AppDownload
            {
                AppId = request.appId,
                DownloadedDate = DateTime.Now.Date
            };

            _repository.Appdownload.Add(entity);
            await _repository.CommitAsync();

            return entity.Id;
        }
    }
}
