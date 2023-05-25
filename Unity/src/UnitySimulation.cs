using UnityEngine.SceneManagement;

namespace Markwardt;

public class UnitySimulation : ISimulation3d
{
    public IEnumerable<IObject3d> Objects => SceneManager.GetActiveScene().GetRootGameObjects().Select(x => x.GetComponent<UnityObject>()).WhereNotNull().Cast<IObject3d>();

    public bool IsDisposed => throw new NotImplementedException();

    public IObject3d Create(string name)
    {
        throw new NotImplementedException();
    }

    public void Destroy(IObject3d obj)
        => GameObject.Destroy(((UnityObject)obj).gameObject);

    public void Dispose()
    {
        foreach (IObject3d obj in Objects)
    }

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }
}