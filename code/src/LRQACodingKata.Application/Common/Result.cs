namespace LRQACodingKata.Application.Common
{
    public class Result
    {
        public bool Succeeded { get; init; }

        public string? Id { get; init; }

        public static Result Success(string? id = null) => new()
        {
            Succeeded = true,
            Id = id
        };

        public static Result NoContent() => new()
        {
            Succeeded = true
        };
    }

    public class Result<T>
    {
        public bool Succeeded { get; init; }

        public T? Value { get; init; }

        public static Result<T> Success(T value) => new() { Succeeded = true, Value = value };
    }
}
