namespace Markwardt;

public interface IStreamSerializer
{
    ValueTask Serialize(object obj, Stream destination);
    ValueTask<object> Deserialize(Stream source);
}