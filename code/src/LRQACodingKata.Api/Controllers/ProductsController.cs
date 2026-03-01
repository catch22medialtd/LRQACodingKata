using LRQACodingKata.Api.Contracts.Responses;
using LRQACodingKata.Api.Controllers.Base;
using LRQACodingKata.Application.Features.Product.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LRQACodingKata.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController(IMediator mediator) : BaseApiController(mediator)
    {
        [HttpGet]
        [ProducesResponseType(typeof(ProductGetListQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductGetListQueryResponse>> Get()
        {
            var query = new ProductGetListQuery();
            var result = await Mediator.Send(query);
            var response = ProductGetListQueryResponse.From(result.Value!);

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductGetByIdQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductGetByIdQueryResponse>> GetById(int id)
        {
            var query = new ProductGetByIdQuery(id);
            var result = await Mediator.Send(query);

            return Ok(ProductGetByIdQueryResponse.From(result.Value!));
        }
    }
}