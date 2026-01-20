using Godot;

public class AOEHex
{
	public Vector2I LocalCoords { get; }
	public AOEHexType Type { get; }
	public string CustomMark { get; }
	public string IconPath { get; }

	public AOEHex(Vector2I localCoords, AOEHexType type, string customMark = null, string iconPath = null)
	{
		LocalCoords = localCoords;
		Type = type;
		CustomMark = customMark;
		IconPath = iconPath;
	}
}