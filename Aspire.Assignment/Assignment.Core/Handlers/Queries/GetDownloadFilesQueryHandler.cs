using Assignment.Contracts.Data;
using Assignment.Contracts.DTO;
using Assignment.Core.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Core.Handlers.Queries
{

    public class GetDownloadFilesQuery : IRequest<string>
    {
        public IFormFile Files { get; }
        public GetDownloadFilesQuery(IFormFile files)
        {
            this.Files = files;
        }

    }

    public class GetDownloadFilesQueryHandler : IRequestHandler<GetDownloadFilesQuery, string>
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;

        public GetDownloadFilesQueryHandler(IUnitOfWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<string> Handle(GetDownloadFilesQuery request, CancellationToken cancellationToken)
        {
            IFormFile files = request.Files;

            var folderName = Path.Combine("Resources");
            var path = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (files.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(files.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(path, fileName);
                var dbPath = Path.Combine(folderName, fileName);
                var memory = new MemoryStream();
                using (var stream = new FileStream(fullPath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

            }
            return path;
        }
    }
}
