using Application.Dtos;
using FluentValidation;

namespace Application.Validators
{
    public class CustomerValidator : AbstractValidator<CustomerCreateDto>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es requerido.")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es requerido.")
                .EmailAddress().WithMessage("El email no es válido.")
                .MaximumLength(150);

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
        }
    }
}
