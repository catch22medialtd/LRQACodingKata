using FluentValidation;
using LRQACodingKata.Core.Constants;

namespace LRQACodingKata.Application.Features.Product.Commands
{
    public class ProductCreateCommandValidator : AbstractValidator<ProductCreateCommand>
    {
        public ProductCreateCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(EntityPropertyLengths.Product_Name);

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("'{PropertyName}' must be greater than 0.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("'{PropertyName}' must be greater than or equal to 0.");
        }
    }
}