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
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace Assignment.Core.Handlers.Commands
{
    public class UpdateAppCommand : IRequest<int>
    {
        public CreateAppDTO Model { get; }
        public IFormFile Files { get; }
        public UpdateAppCommand(CreateAppDTO model, IFormFile files)
        {
            this.Model = model;
            this.Files = files;
        }
    }

    public class UpdateAppCommandHandler : IRequestHandler<UpdateAppCommand, int>
    {
        private readonly IUnitOfWork _repository;
        private readonly IValidator<CreateAppDTO> _validator;

        public UpdateAppCommandHandler(IUnitOfWork repository, IValidator<CreateAppDTO> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<int> Handle(UpdateAppCommand request, CancellationToken cancellationToken)
        {
            CreateAppDTO model = request.Model;
            IFormFile files = request.Files;

            var result = _validator.Validate(model);

            var app = await Task.FromResult(_repository.App.Get(model.Id));

            var folderName = Path.Combine("Resources");
            var path = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (files != null)
            {
                if (files.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(files.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(path, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        files.CopyTo(stream);
                        stream.Close();
                    }
                }
            }

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(x => x.ErrorMessage).ToArray();
                throw new InvalidRequestBodyException
                {
                    Errors = errors
                };
            }
            var entity = new App
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = Convert.ToDecimal(model.Price),
                Type = model.Type,
                Files = model.Files,
            };

            _repository.App.Update(entity);
            await _repository.CommitAsync();
            return app.Id;
        }
    }
}
