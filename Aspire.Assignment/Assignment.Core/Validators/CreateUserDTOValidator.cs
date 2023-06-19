using Assignment.Contracts.DTO;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Assignment.Core.Validators
{
    [ExcludeFromCodeCoverage]
    public class CreateuserDTOValidator : AbstractValidator<CreateUserDTO>
    {
        public CreateuserDTOValidator()
        {
            RuleFor(x => x.Firstname).NotEmpty().WithMessage("Firstname is required");
            RuleFor(x => x.Lastname).NotEmpty().WithMessage("Lastname is required");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Provide passsword");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
        }
    }
}
