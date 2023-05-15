namespace Markwardt;

public class UnityConsoleWriter : ConsoleWriter
{
    protected override void WriteTarget(object target, bool isError)
    {
        if (isError)
        {
            Debug.LogError(target.ToString());
        }
        else
        {
            Debug.Log(target.ToString());
        }
    }
}