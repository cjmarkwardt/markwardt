namespace Markwardt;

public interface IJsonProvider
{
    IJsonSerializer CreateSerializer(ITypeSerializer? types = null, bool isIndented = false);
}