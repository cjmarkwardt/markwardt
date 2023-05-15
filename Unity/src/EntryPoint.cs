namespace Markwardt;

public abstract class EntryPoint<TApplication> : MonoBehaviour
    where TApplication : IApplication, new()
{
    private async void Awake()
        => await new TApplication().RunWithCommandLineArguments(new UnityPackage());
}