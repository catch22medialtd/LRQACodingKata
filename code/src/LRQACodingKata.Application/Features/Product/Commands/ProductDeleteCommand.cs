using LRQACodingKata.Application.Common;
using LRQACodingKata.Core.Repositories;
using MediatR;

namespace LRQACodingKata.Application.Features.Product.Commands
{
    public record ProductDeleteCommand(int Id) : IRequest<Result>;

    public class ProductDeleteCommandHandler(IRepository<Core.Entities.Product> repository)
        : IRequestHandler<ProductDeleteCommand, Result>
    {
        private readonly IRepository<Core.Entities.Product> _repository = repository;

        public async Task<Result> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (product is not null)
            {
                _repository.Delete(product);

                await _repository.SaveChangesAsync(cancellationToken);
            }

            return Result.NoContent();
        }
    }
}