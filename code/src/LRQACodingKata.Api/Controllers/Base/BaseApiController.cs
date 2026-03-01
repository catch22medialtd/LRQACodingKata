using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LRQACodingKata.Api.Controllers.Base
{
    [ApiController]
    public abstract class BaseApiController(IMediator mediator) : ControllerBase
    {
        protected readonly IMediator Mediator = mediator;
    }
}