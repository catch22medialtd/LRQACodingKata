using FluentValidation;
using LRQACodingKata.Application.Features.Product.Commands;
using LRQACodingKata.Core.Constants;

namespace LRQACodingKata.Application.Features.Product.Validators
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
                .InclusiveBetween(EntityPropertyRanges.Product_Price_Min, EntityPropertyRanges.Product_Price_Max);

            RuleFor(x => x.Stock)
                .InclusiveBetween(EntityPropertyRanges.Product_Stock_Min, EntityPropertyRanges.Product_Stock_Max);
        }
    }
}