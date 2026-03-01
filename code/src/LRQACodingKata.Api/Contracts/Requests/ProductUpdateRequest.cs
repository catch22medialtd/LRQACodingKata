namespace LRQACodingKata.Api.Contracts.Requests
{
    public class ProductUpdateRequest
    {
        public required string Name { get; init; }

        public decimal Price { get; init; }

        public int Stock { get; init; }
    }
}