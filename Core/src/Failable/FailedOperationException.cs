namespace Markwardt;

public class FailedOperationException : Exception
{
    public FailedOperationException(string message)
        : this(new Failure(message)) { }

    public FailedOperationException(Failure failure)
        : base(failure.Message)
    {
        Failure = failure;
    }

    public Failure Failure { get; }
}