using LRQACodingKata.Application.Features.Product.Dtos;
using LRQACodingKata.Application.Features.Product.Queries;

namespace LRQACodingKata.Api.Contracts.Responses
{
    public class ProductGetByIdResponse
    {
        public ProductDto Product { get; set; } = null!;

        public static ProductGetByIdResponse From(ProductGetByIdQueryResult result)
        {
            return new ProductGetByIdResponse
            {
                Product = result.Product
            };
        }
    }
}