using Assignment.Contracts.DTO;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Assignment.Core.Validators
{
    [ExcludeFromCodeCoverage]
    public class CreateAppDTOValidator : AbstractValidator<CreateAppDTO>
    {
        public CreateAppDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Provide a brief description about the App");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required");
            RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required");
            RuleFor(x=> x.Files).NotEmpty().WithMessage("Files is required");
        }
    }
}
