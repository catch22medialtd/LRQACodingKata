using FluentAssertions;
using LRQACodingKata.Api.Contracts.Responses;
using LRQACodingKata.Api.Controllers;
using LRQACodingKata.Application.Common;
using LRQACodingKata.Application.Features.Product.Dtos;
using LRQACodingKata.Application.Features.Product.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace LRQACodingKata.UnitTests.Api.Controllers
{
    public class ProductsControllerTests
    {
        private readonly IMediator _mediator;
        private readonly ProductsController _sut;

        public ProductsControllerTests()
        {
            _mediator = Substitute.For<IMediator>();
            _sut = new ProductsController(_mediator);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOk_WithMappedProducts()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new() { Id = 1, Name = "Product A", Price = 9.99m, Stock = 10 },
                new() { Id = 2, Name = "Product B", Price = 19.99m, Stock = 5 }
            };

            var queryResult = Result<ProductGetListQueryResult>.Success(new ProductGetListQueryResult
            {
                Products = products
            });

            _mediator
                .Send(Arg.Any<ProductGetListQuery>(), Arg.Any<CancellationToken>())
                .Returns(queryResult);

            // Act
            var actionResult = await _sut.GetProducts();

            // Assert
            var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ProductGetListResponse>().Subject;

            response.Products.Should().HaveCount(2);
            response.Products.Should().BeEquivalentTo(products);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOk_WithEmptyList_WhenNoProductsExist()
        {
            // Arrange
            var queryResult = Result<ProductGetListQueryResult>.Success(new ProductGetListQueryResult
            {
                Products = []
            });

            _mediator
                .Send(Arg.Any<ProductGetListQuery>(), Arg.Any<CancellationToken>())
                .Returns(queryResult);

            // Act
            var actionResult = await _sut.GetProducts();

            // Assert
            var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ProductGetListResponse>().Subject;

            response.Products.Should().BeEmpty();
        }

        [Fact]
        public async Task GetProducts_ShouldSendProductGetListQuery()
        {
            // Arrange
            var queryResult = Result<ProductGetListQueryResult>.Success(new ProductGetListQueryResult());

            _mediator
                .Send(Arg.Any<ProductGetListQuery>(), Arg.Any<CancellationToken>())
                .Returns(queryResult);

            // Act
            await _sut.GetProducts();

            // Assert
            await _mediator.Received(1).Send(Arg.Any<ProductGetListQuery>(), Arg.Any<CancellationToken>());
        }
    }
}