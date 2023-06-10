using MediatR;
using Assignment.Contracts.Data;
using Assignment.Contracts.DTO;
using Assignment.Contracts.Data.Entities;
using FluentValidation;
using System.Text.Json;
using Assignment.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Assignment.Contracts.Data.Repositories;

namespace Assignment.Providers.Handlers.Commands
{
    public class CreateUserCommand : IRequest<int>
    {
        public CreateUserDTO Model { get; }
        public CreateUserCommand(CreateUserDTO model)
        {
            this.Model = model;
        }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUnitOfWork _repository;
        private readonly IValidator<CreateUserDTO> _validator;
        private readonly IPasswordHasher<User> _passwordHasher;

        public CreateUserCommandHandler(IUnitOfWork repository, IValidator<CreateUserDTO> validator, IPasswordHasher<User> passwordHasher)
        {
            _repository = repository;
            _validator = validator;
            _passwordHasher = passwordHasher;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            CreateUserDTO model = request.Model;

            var result = _validator.Validate(model);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(x => x.ErrorMessage).ToArray();
                throw new InvalidRequestBodyException
                {
                    Errors = errors
                };
            }

            var entity = new User
            {
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Username = model.Username,
                Email = model.Email,   
                Address = model.Address,
                Role= model.Role
            };

             entity.Password= _passwordHasher.HashPassword(entity, model.Password);
            _repository.User.Add(entity);
            await _repository.CommitAsync();

            return entity.Id;
        }
    }
}