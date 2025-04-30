using Application.Dtos;
using FluentValidation;

namespace Application.Validators
{
    public class OrderValidator : AbstractValidator<OrderDto>
    {
        public OrderValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("El ID del cliente es requerido.");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("El total debe ser mayor que cero.");

            RuleFor(x => x.OrderStatusId)
                .NotEmpty().WithMessage("El estado del pedido es requerido.");

            RuleFor(x => x.OrderItems)
                .NotNull().WithMessage("Debe incluir al menos un ítem.")
                .Must(items => items.Any()).WithMessage("La orden debe tener al menos un ítem.");

            RuleForEach(x => x.OrderItems).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .NotEmpty().WithMessage("El ID del producto es requerido.");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("La cantidad debe ser mayor que cero.");
            });
        }
    }
}
