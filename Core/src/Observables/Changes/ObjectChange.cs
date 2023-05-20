namespace Markwardt;

public interface IObjectChange : IObservedChange
{
    IObservableTarget Member { get; }
    IObservedChange MemberChange { get; }
}

public record ObjectChange(IObservableTarget Target, IObservableTarget Member, IObservedChange MemberChange) : IObjectChange;