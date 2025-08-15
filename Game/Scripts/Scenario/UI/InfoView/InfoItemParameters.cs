public abstract class InfoItemParameters<T> : InfoItemParameters
	where T : HexObject
{
	public T HexObject { get; }

	public InfoItemParameters(T hexObject)
	{
		HexObject = hexObject;
	}
}

public abstract class InfoItemParameters
{
	public abstract string ScenePath { get; }
}