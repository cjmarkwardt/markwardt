using UnityEngine.SceneManagement;

namespace Markwardt;

public class UnityRoot : IWorldRoot
{
    public IEnumerable<ISimulationObject> Objects => SceneManager.GetActiveScene().GetRootGameObjects()
}