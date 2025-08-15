using Godot;

public class AOEHex
{
	public Vector2I LocalCoords { get; }
	public AOEHexType Type { get; }

	public AOEHex(Vector2I localCoords, AOEHexType type)
	{
		LocalCoords = localCoords;
		Type = type;
	}
}