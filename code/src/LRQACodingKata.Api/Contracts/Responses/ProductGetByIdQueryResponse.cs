using LRQACodingKata.Application.Features.Product.Dtos;
using LRQACodingKata.Application.Features.Product.Queries;

namespace LRQACodingKata.Api.Contracts.Responses
{
    public class ProductGetByIdQueryResponse
    {
        public ProductDto Product { get; set; } = null!;

        public static ProductGetByIdQueryResponse From(ProductGetByIdQueryResult result)
        {
            return new ProductGetByIdQueryResponse
            {
                Product = result.Product
            };
        }
    }
}