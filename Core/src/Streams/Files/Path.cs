namespace Markwardt;

public static class PathUtils
{
    public static Path ToPath(this string text, string separator)
        => new Path(text.Split(separator));
}

public class Path : IEquatable<Path?>
{
    public static bool operator ==(Path? a, Path? b)
        => (a == null && b == null) || (a != null && a.Equals(b));

    public static bool operator !=(Path? a, Path? b)
        => !(a == b);

    public static Path operator +(Path a, Path b)
        => a.Append(b);

    public static Path operator +(Path a, string b)
        => a.Append(b);

    public Path(IEnumerable<string> parts)
    {
        Parts = parts.ToList();
    }

    public Path(string part, params string[] parts)
        : this(parts.Prepend(part)) { }

    public IReadOnlyList<string> Parts { get; }

    public Path Root => Parts.Count == 1 ? this : new Path(Parts.First());
    public Path? Parent => Parts.Count == 1 ? null : new Path(Parts.Take(Parts.Count - 1));
    public string Name => Parts.Last();

    public Path Append(IEnumerable<string> parts)
        => new Path(Parts.Concat(parts));

    public Path Append(string part, params string[] parts)
        => Append(parts.Prepend(part));

    public Path Append(Path path)
        => Append(path.Parts);

    public Path Prepend(IEnumerable<string> parts)
        => new Path(parts.Concat(Parts));

    public Path Prepend(string part, params string[] parts)
        => Prepend(parts.Prepend(part));

    public Path Prepend(Path path)
        => Prepend(path.Parts);

    public string ToString(string separator)
        => string.Join(separator, Parts);

    public override string ToString()
        => ToString("/");

    public bool Equals(Path? other)
        => other is not null && Parts.SequenceEqual(other.Parts);

    public override bool Equals(object obj)
        => obj is Path other && Equals(other);

    public override int GetHashCode()
    {
        HashCode hash = new();
        foreach (string part in Parts)
        {
            hash.Add(part);
        }

        return hash.ToHashCode();
    }
}