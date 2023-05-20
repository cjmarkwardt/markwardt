using UnityEngine.SceneManagement;

namespace Markwardt;

public class UnityRoot : IWorldRoot
{
    public IEnumerable<IWorldObject> Objects => SceneManager.GetActiveScene().GetRootGameObjects()
}