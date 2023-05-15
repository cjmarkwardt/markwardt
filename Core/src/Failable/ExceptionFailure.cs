namespace Markwardt;

public static class ExceptionFailureUtils
{
    public static Failure AsFailure(this Exception exception, string? message = null)
        => new ExceptionFailure(exception, message ?? exception.Message);

    public static Failable AsFailable(this Exception exception, string? message = null)
        => exception.AsFailure(message);

    public static Failable<TResult> AsFailable<TResult>(this Exception exception, string? message = null)
        => Failable.Fail<TResult>(exception.AsFailure(message));
}

public record ExceptionFailure(Exception Exception, string Message) : Failure(Message)
{
    public static implicit operator ExceptionFailure(Exception exception)
        => new ExceptionFailure(exception);

    public ExceptionFailure(Exception exception)
        : this(exception, exception.Message) { }

    public override Exception AsException()
        => Exception;

    public override string ToDetailedString()
        => Exception.ToString();
}