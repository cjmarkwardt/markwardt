namespace Markwardt;

public interface ITextSerializer
{
    string Serialize(object obj);
    object Deserialize(string text);
}