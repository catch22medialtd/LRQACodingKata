using FluentValidation;
using LRQACodingKata.Core.Constants;

namespace LRQACodingKata.Application.Features.Product.Commands
{
    public class ProductUpdateCommandValidator : AbstractValidator<ProductUpdateCommand>
    {
        public ProductUpdateCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

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