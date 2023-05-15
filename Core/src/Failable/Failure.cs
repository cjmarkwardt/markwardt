namespace Markwardt;

public record Failure(string Message)
{
    public static implicit operator string(Failure failure)
        => failure.Message;

    public static implicit operator Exception(Failure failure)
        => failure.AsException();

    public virtual Exception AsException()
        => new FailedOperationException(Message);

    public override string ToString()
        => Message;

    public virtual string ToDetailedString()
        => Message;
}