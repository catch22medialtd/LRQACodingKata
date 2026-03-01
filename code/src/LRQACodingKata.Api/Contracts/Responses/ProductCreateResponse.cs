using LRQACodingKata.Application.Features.Product.Commands;
using LRQACodingKata.Application.Features.Product.Dtos;

namespace LRQACodingKata.Api.Contracts.Responses
{
    public class ProductCreateResponse
    {
        public ProductDto Product { get; set; } = null!;

        public static ProductCreateResponse From(ProductCreateCommandResult result)
        {
            return new ProductCreateResponse
            {
                Product = result.Product
            };
        }
    }
}