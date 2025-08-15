public class XPLevelValues
{
	public int[] Values { get; }

	public static XPLevelValues Default { get; } = new XPLevelValues([45, 95, 150, 210, 275, 345, 420, 500]);

	public XPLevelValues(int[] values)
	{
		Values = values;
	}
}