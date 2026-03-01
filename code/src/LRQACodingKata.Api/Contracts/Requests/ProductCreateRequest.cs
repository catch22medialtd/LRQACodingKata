namespace LRQACodingKata.Api.Contracts.Requests
{
    public class ProductCreateRequest
    {
        public required string Name { get; init; }

        public decimal Price { get; init; }

        public int Stock { get; init; }
    }
}