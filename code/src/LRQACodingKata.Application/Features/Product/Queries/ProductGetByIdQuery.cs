using LRQACodingKata.Application.Common;
using LRQACodingKata.Application.Exceptions;
using LRQACodingKata.Application.Features.Product.Dtos;
using LRQACodingKata.Core.Repositories;
using MediatR;

namespace LRQACodingKata.Application.Features.Product.Queries
{
    public record ProductGetByIdQuery(int Id) : IRequest<Result<ProductGetByIdQueryResult>>;

    public class ProductGetByIdQueryResult
    {
        public ProductDto Product { get; set; } = null!;
    }

    public class ProductGetByIdQueryHandler(IRepository<Core.Entities.Product> repository)
        : IRequestHandler<ProductGetByIdQuery, Result<ProductGetByIdQueryResult>>
    {
        private readonly IRepository<Core.Entities.Product> _repository = repository;

        public async Task<Result<ProductGetByIdQueryResult>> Handle(ProductGetByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Core.Entities.Product), request.Id);

            return Result<ProductGetByIdQueryResult>.Success(new ProductGetByIdQueryResult
            {
                Product = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock
                }
            });
        }
    }
}