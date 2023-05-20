namespace Markwardt;

public class UnityBehavior : WorldBehavior
{
    protected GameObject GameObject => ((UnityObject)base.Object).gameObject;
}