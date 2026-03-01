using LRQACodingKata.Application.Features.Product.Dtos;
using LRQACodingKata.Application.Features.Product.Queries;

namespace LRQACodingKata.Api.Contracts.Responses
{
    public class ProductGetListQueryResponse
    {
        public IReadOnlyList<ProductDto> Products { get; set; } = [];

        public static ProductGetListQueryResponse From(ProductGetListQueryResult result)
        {
            return new ProductGetListQueryResponse
            {
                Products = [.. result.Products]
            };
        }
    }
}