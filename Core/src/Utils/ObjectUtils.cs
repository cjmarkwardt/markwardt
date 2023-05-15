namespace Markwardt;

public static class ObjectUtils
{
    public static bool NullableEquals(this object? obj, object? other)
        => (obj == null && other == null) || (obj != null && obj.Equals(other));
}