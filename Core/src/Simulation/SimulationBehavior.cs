namespace Markwardt;

public interface ISimulationBehavior : IFullDisposable
{
    void Start();
    void Update();
}