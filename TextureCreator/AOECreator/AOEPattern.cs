using System.Collections.Generic;

public class AOEPattern
{
	public string FileName { get; }
	public List<AOEHex> LocalHexes { get; }

	public AOEPattern(string fileName, List<AOEHex> localHexes)
	{
		FileName = fileName;
		LocalHexes = localHexes;
	}
}