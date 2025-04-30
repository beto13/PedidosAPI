using Application.Dtos;
using FluentValidation;

namespace Application.Validators
{
    public class OrderItemValidator : AbstractValidator<OrderItemDto>
    {
        public OrderItemValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("El ID del pedido es requerido.");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("El ID del producto es requerido.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor que cero.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("El precio unitario debe ser mayor que cero.");
        }
    }
}
