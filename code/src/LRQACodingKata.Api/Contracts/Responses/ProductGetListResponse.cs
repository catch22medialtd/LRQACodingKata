using LRQACodingKata.Application.Features.Product.Dtos;
using LRQACodingKata.Application.Features.Product.Queries;

namespace LRQACodingKata.Api.Contracts.Responses
{
    public class ProductGetListResponse
    {
        public IReadOnlyList<ProductDto> Products { get; set; } = [];

        public static ProductGetListResponse From(ProductGetListQueryResult result)
        {
            return new ProductGetListResponse
            {
                Products = [.. result.Products]
            };
        }
    }
}