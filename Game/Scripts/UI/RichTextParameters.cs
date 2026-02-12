using Godot;

public struct RichTextParameters
{
	public int FontSize { get; }
	public Color Color { get; }

	public RichTextParameters(int fontSize, Color color)
	{
		FontSize = fontSize;
		Color = color;
	}
}