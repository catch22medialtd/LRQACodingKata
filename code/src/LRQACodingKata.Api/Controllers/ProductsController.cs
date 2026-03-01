using LRQACodingKata.Api.Contracts.Requests;
using LRQACodingKata.Api.Contracts.Responses;
using LRQACodingKata.Api.Controllers.Base;
using LRQACodingKata.Application.Features.Product.Commands;
using LRQACodingKata.Application.Features.Product.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LRQACodingKata.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController(IMediator mediator) : BaseApiController(mediator)
    {
        [HttpGet]
        [ProducesResponseType(typeof(ProductGetListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductGetListResponse>> Get()
        {
            var query = new ProductGetListQuery();
            var result = await Mediator.Send(query);
            var response = ProductGetListResponse.From(result.Value!);

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductGetByIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductGetByIdResponse>> GetById(int id)
        {
            var query = new ProductGetByIdQuery(id);
            var result = await Mediator.Send(query);
            var response = ProductGetByIdResponse.From(result.Value!);

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductCreateResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductCreateResponse>> Create([FromBody] ProductCreateRequest request)
        {
            var command = new ProductCreateCommand(request.Name, request.Price, request.Stock);
            var result = await Mediator.Send(command);
            var response = ProductCreateResponse.From(result.Value!);

            return CreatedAtAction(nameof(GetById), new { id = response.Product.Id }, response);
        }
    }
}