using FluentValidation.Results;

namespace LRQACodingKata.Application.Exceptions
{
    public sealed class ValidationException : Exception
    {
        private const string DefaultMessage = "One or more validation failures have occurred.";

        public IReadOnlyDictionary<string, string[]> Errors { get; }

        public ValidationException() : base(DefaultMessage)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IDictionary<string, string[]> errors) : this()
        {
            Errors = (IReadOnlyDictionary<string, string[]>)errors;
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures
                .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                .ToDictionary(x => x.Key, x => x.ToArray());
        }
    }
}