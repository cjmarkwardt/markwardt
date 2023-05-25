namespace Markwardt;

public class UnityBehavior : SimulationBehavior
{
    protected GameObject GameObject => ((UnityObject)base.Object).gameObject;
}