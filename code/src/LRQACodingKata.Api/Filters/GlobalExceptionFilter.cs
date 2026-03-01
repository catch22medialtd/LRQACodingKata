using LRQACodingKata.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LRQACodingKata.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly Dictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(Application.Exceptions.ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(ConflictException), HandleConflictException }
            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            var type = context.Exception.GetType();

            if (_exceptionHandlers.TryGetValue(type, out var handler))
            {
                handler.Invoke(context);
                context.ExceptionHandled = true;

                return;
            }

            if (type.BaseType != null && _exceptionHandlers.TryGetValue(type.BaseType, out var baseTypeHandler))
            {
                baseTypeHandler.Invoke(context);
                context.ExceptionHandled = true;

                return;
            }

            HandleUnknownException(context);
            AppendRequestData(context);
        }

        private static void HandleValidationException(ExceptionContext context)
        {
            var exception = (ValidationException)context.Exception;

            var details = new ValidationProblemDetails(exception.Errors.ToDictionary(x => x.Key, x => x.Value))
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Detail = context.Exception.Message
            };

            context.Result = new ObjectResult(details);
        }

        private static void HandleNotFoundException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Detail = context.Exception.Message
            };

            context.Result = new ObjectResult(details);
        }

        private static void HandleConflictException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Conflict.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
                Detail = context.Exception.Message
            };

            context.Result = new ObjectResult(details);
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "500 Internal Server Error: An unhandled exception occurred.");

            var details = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            context.Result = new ObjectResult(details);
            context.ExceptionHandled = true;
        }

        private static void AppendRequestData(ExceptionContext context)
        {
            if (context.Result is not ObjectResult { Value: ProblemDetails problemDetails } result)
            {
                return;
            }

            result.StatusCode = problemDetails.Status;

            var requestId = context.HttpContext.TraceIdentifier;

            problemDetails.Extensions.Add("requestId", requestId);
            context.ExceptionHandled = true;
        }
    }
}
