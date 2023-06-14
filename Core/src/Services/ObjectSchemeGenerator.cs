namespace Markwardt;

public interface IObjectSchemeGenerator
{
    Maybe<IObjectScheme> Generate(ObjectTag tag);
}