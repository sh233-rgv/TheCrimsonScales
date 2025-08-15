using System.Collections.Generic;

public class AOEPattern
{
	public List<AOEHex> Hexes { get; }

	public AOEPattern(List<AOEHex> hexes)
	{
		Hexes = hexes;
	}
}