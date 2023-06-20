using MediatR;
using Assignment.Contracts.Data;
using Assignment.Contracts.DTO;
using Assignment.Contracts.Data.Entities;
using FluentValidation;
using System.Text.Json;
using Assignment.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace Assignment.Providers.Handlers.Commands
{
    public class CreateAppCommand : IRequest<int>
    {
        public CreateAppDTO Model { get; }
        public IFormFile Files { get; }

        public CreateAppCommand(CreateAppDTO model, IFormFile files)
        {
            this.Model = model;
            this.Files = files;
        }
    }

    public class CreateAppCommandHandler : IRequestHandler<CreateAppCommand, int>
    {
        private readonly IUnitOfWork _repository;
        private readonly IValidator<CreateAppDTO> _validator;

        public CreateAppCommandHandler(IUnitOfWork repository, IValidator<CreateAppDTO> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<int> Handle(CreateAppCommand request, CancellationToken cancellationToken)
        {
            CreateAppDTO model = request.Model;
            IFormFile files = request.Files;

            var result = _validator.Validate(model);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(x => x.ErrorMessage).ToArray();
                throw new InvalidRequestBodyException
                {
                    Errors = errors
                };
            }


            var folderName = Path.Combine("Resources");
            var path = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            if (path == null)
            {
                throw new ArgumentNullException("Path not found!");
            }

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

            var entity = new App
            {
                Name = model.Name,
                Description = model.Description,
                Price = Convert.ToDecimal(model.Price),
                Type = model.Type,
                Files = model.Files,
            };

            _repository.App.Add(entity);
            await _repository.CommitAsync();

            return entity.Id;
        }
    }
}