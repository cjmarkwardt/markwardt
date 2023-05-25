namespace Markwardt;

public interface ISimulationTransform<TDistance, TOrientation>
{
    TDistance Position { get; set; }
    TDistance Scale { get; set; }
    TOrientation Rotation { get; set; }
}