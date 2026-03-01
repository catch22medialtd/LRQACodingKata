using LRQACodingKata.Application.Common;
using LRQACodingKata.Application.Features.Product.Dtos;
using LRQACodingKata.Core.Repositories;
using MediatR;

namespace LRQACodingKata.Application.Features.Product.Commands
{
    public record ProductCreateCommand(string Name, decimal Price, int Stock) : IRequest<Result<ProductCreateCommandResult>>;

    public class ProductCreateCommandResult
    {
        public ProductDto Product { get; set; } = null!;
    }

    public class ProductCreateCommandHandler(IRepository<Core.Entities.Product> repository)
        : IRequestHandler<ProductCreateCommand, Result<ProductCreateCommandResult>>
    {
        private readonly IRepository<Core.Entities.Product> _repository = repository;

        public async Task<Result<ProductCreateCommandResult>> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
        {
            // Requirements dont specify whether a duplicate product name is allowed,
            // so we will allow it for now. If needed, we can add a check here to prevent duplicates.

            var product = new Core.Entities.Product
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            };

            await _repository.AddAsync(product, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Result<ProductCreateCommandResult>.Success(new ProductCreateCommandResult
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