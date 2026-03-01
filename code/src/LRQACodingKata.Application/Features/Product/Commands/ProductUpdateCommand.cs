using LRQACodingKata.Application.Common;
using LRQACodingKata.Application.Exceptions;
using LRQACodingKata.Core.Repositories;
using MediatR;

namespace LRQACodingKata.Application.Features.Product.Commands
{
    public record ProductUpdateCommand(int Id, string Name, decimal Price, int Stock) : IRequest<Result>;

    public class ProductUpdateCommandHandler(IRepository<Core.Entities.Product> repository)
        : IRequestHandler<ProductUpdateCommand, Result>
    {
        private readonly IRepository<Core.Entities.Product> _repository = repository;

        public async Task<Result> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Core.Entities.Product), request.Id);

            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;
            _repository.Update(product);
            
            await _repository.SaveChangesAsync(cancellationToken);

            return Result.NoContent();
        }
    }
}