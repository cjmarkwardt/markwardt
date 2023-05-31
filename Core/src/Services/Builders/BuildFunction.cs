namespace Markwardt;

public delegate ValueTask<object> BuildFunction(IObjectContainer container, IArgumentGenerator? arguments = null);