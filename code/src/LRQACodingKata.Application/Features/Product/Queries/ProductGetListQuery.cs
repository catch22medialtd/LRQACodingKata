using LRQACodingKata.Application.Common;
using LRQACodingKata.Application.Features.Product.Dtos;
using LRQACodingKata.Core.Repositories;
using MediatR;

namespace LRQACodingKata.Application.Features.Product.Queries
{
    public record ProductGetListQuery : IRequest<Result<ProductGetListQueryResult>>
    {
    }

    public class ProductGetListQueryResult
    {
        public List<ProductDto> Products { get; set; } = [];
    }

    public class ProductGetListQueryHandler(IRepository<Core.Entities.Product> repository) 
        : IRequestHandler<ProductGetListQuery, Result<ProductGetListQueryResult>>
    {
        private readonly IRepository<Core.Entities.Product> _repository = repository;

        public async Task<Result<ProductGetListQueryResult>> Handle(ProductGetListQuery request, CancellationToken cancellationToken)
        {
            var productDtos = await _repository
                .GetAllProjectedAsync(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock
                }, cancellationToken);

            return Result<ProductGetListQueryResult>.Success(new ProductGetListQueryResult
            {
                Products = [.. productDtos]
            });
        }
    }
}