namespace Markwardt;

public static class UnityUtils
{
    public static TComponent RequireComponent<TComponent>(this GameObject gameObject)
        where TComponent : Component
        => gameObject.GetComponent<TComponent>() ?? throw new InvalidOperationException($"Object {gameObject.name} does not have component {typeof(TComponent)}");

    public static Vector3 ToGeneralVector(this UnityVector3 vector)
        => new Vector3(vector.x, vector.y, vector.z);

    public static UnityVector3 ToUnityVector(this Vector3 vector)
        => new UnityVector3(vector.X, vector.Y, vector.Z);

    public static Vector3 ToGeneralVector(this Quaternion quaternion)
        => quaternion.eulerAngles.ToGeneralVector();

    public static Quaternion ToQuaternion(this Vector3 vector)
        => Quaternion.Euler(vector.ToUnityVector());
}