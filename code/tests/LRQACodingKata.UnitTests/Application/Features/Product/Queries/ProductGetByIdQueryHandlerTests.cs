using FluentAssertions;
using LRQACodingKata.Application.Exceptions;
using LRQACodingKata.Application.Features.Product.Dtos;
using LRQACodingKata.Application.Features.Product.Queries;
using LRQACodingKata.Core.Repositories;
using NSubstitute;

namespace LRQACodingKata.UnitTests.Application.Features.Product.Queries
{
    public class ProductGetByIdQueryHandlerTests
    {
        private readonly IRepository<Core.Entities.Product> _repository;
        private readonly ProductGetByIdQueryHandler _sut;

        public ProductGetByIdQueryHandlerTests()
        {
            _repository = Substitute.For<IRepository<Core.Entities.Product>>();
            _sut = new ProductGetByIdQueryHandler(_repository);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WithMappedProduct_WhenProductExists()
        {
            // Arrange
            const int productId = 1;
            var product = new Core.Entities.Product
            {
                Id = productId,
                Name = "Test Product",
                Price = 19.99m,
                Stock = 10
            };

            var query = new ProductGetByIdQuery(productId);
            var cancellationToken = CancellationToken.None;

            _repository
                .GetByIdAsync(productId, cancellationToken)
                .Returns(product);

            // Act
            var result = await _sut.Handle(query, cancellationToken);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value!.Product.Should().BeEquivalentTo(new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            });
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            const int productId = 999;
            var query = new ProductGetByIdQuery(productId);
            var cancellationToken = CancellationToken.None;

            _repository
                .GetByIdAsync(productId, cancellationToken)
                .Returns((Core.Entities.Product?)null);

            // Act
            var act = async () => await _sut.Handle(query, cancellationToken);

            // Assert
            var exception = await act.Should().ThrowAsync<NotFoundException>();
            exception.Which.Message.Should().Be($"Entity \"Product\" with id ({productId}) was not found.");
        }

        [Fact]
        public async Task Handle_ShouldCallRepository_WithCorrectId()
        {
            // Arrange
            const int productId = 42;
            var product = new Core.Entities.Product
            {
                Id = productId,
                Name = "Test Product",
                Price = 9.99m,
                Stock = 5
            };

            var query = new ProductGetByIdQuery(productId);
            var cancellationToken = CancellationToken.None;

            _repository
                .GetByIdAsync(productId, cancellationToken)
                .Returns(product);

            // Act
            await _sut.Handle(query, cancellationToken);

            // Assert
            await _repository.Received(1).GetByIdAsync(productId, cancellationToken);
        }

        [Fact]
        public async Task Handle_ShouldReturnCorrectProductData_WithAllProperties()
        {
            // Arrange
            const int productId = 123;
            var product = new Core.Entities.Product
            {
                Id = productId,
                Name = "Complex Product Name",
                Price = 999.99m,
                Stock = 0
            };

            var query = new ProductGetByIdQuery(productId);
            var cancellationToken = CancellationToken.None;

            _repository
                .GetByIdAsync(productId, cancellationToken)
                .Returns(product);

            // Act
            var result = await _sut.Handle(query, cancellationToken);

            // Assert
            result.Value!.Product.Id.Should().Be(product.Id);
            result.Value.Product.Name.Should().Be(product.Name);
            result.Value.Product.Price.Should().Be(product.Price);
            result.Value.Product.Stock.Should().Be(product.Stock);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-999)]
        public async Task Handle_ShouldThrowNotFoundException_WhenProductIdIsInvalidAndNotFound(int invalidId)
        {
            // Arrange
            var query = new ProductGetByIdQuery(invalidId);
            var cancellationToken = CancellationToken.None;

            _repository
                .GetByIdAsync(invalidId, cancellationToken)
                .Returns((Core.Entities.Product?)null);

            // Act
            var act = async () => await _sut.Handle(query, cancellationToken);

            // Assert
            var exception = await act.Should().ThrowAsync<NotFoundException>();
            exception.Which.Message.Should().Be($"Entity \"Product\" with id ({invalidId}) was not found.");
        }

        [Fact]
        public async Task Handle_ShouldPassCancellationToken_ToRepository()
        {
            // Arrange
            const int productId = 1;
            var product = new Core.Entities.Product
            {
                Id = productId,
                Name = "Test Product",
                Price = 10.00m,
                Stock = 1
            };

            var query = new ProductGetByIdQuery(productId);
            var cancellationToken = new CancellationToken(true);

            _repository
                .GetByIdAsync(productId, cancellationToken)
                .Returns(product);

            // Act
            await _sut.Handle(query, cancellationToken);

            // Assert
            await _repository.Received(1).GetByIdAsync(productId, cancellationToken);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WithZeroStockProduct()
        {
            // Arrange
            const int productId = 1;
            var product = new Core.Entities.Product
            {
                Id = productId,
                Name = "Out of Stock Product",
                Price = 50.00m,
                Stock = 0
            };

            var query = new ProductGetByIdQuery(productId);
            var cancellationToken = CancellationToken.None;

            _repository
                .GetByIdAsync(productId, cancellationToken)
                .Returns(product);

            // Act
            var result = await _sut.Handle(query, cancellationToken);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Value!.Product.Stock.Should().Be(0);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WithMinimalPriceProduct()
        {
            // Arrange
            const int productId = 1;
            var product = new Core.Entities.Product
            {
                Id = productId,
                Name = "Cheap Product",
                Price = 0.01m,
                Stock = 100
            };

            var query = new ProductGetByIdQuery(productId);
            var cancellationToken = CancellationToken.None;

            _repository
                .GetByIdAsync(productId, cancellationToken)
                .Returns(product);

            // Act
            var result = await _sut.Handle(query, cancellationToken);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Value!.Product.Price.Should().Be(0.01m);
        }
    }
}
