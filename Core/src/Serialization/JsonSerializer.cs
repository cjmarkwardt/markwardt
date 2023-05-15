namespace Markwardt;

public interface IJsonSerializer : ITextSerializer
{
    delegate IJsonSerializer Factory(bool indent = false);
}