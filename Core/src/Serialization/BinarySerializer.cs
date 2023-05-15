namespace Markwardt;

public interface IBinarySerializer
{
    Memory<byte> Serialize(object obj);
    object Deserialize(ReadOnlySpan<byte> data);
}