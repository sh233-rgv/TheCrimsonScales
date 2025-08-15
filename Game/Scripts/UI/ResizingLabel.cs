using Godot;

public partial class ResizingLabel : Label
{
	[Export]
	private int _minSize;
	[Export]
	private int _maxSize;

	private LabelSettings _labelSettings;

	public override void _Ready()
	{
		base._Ready();

		_labelSettings = (LabelSettings)LabelSettings.Duplicate();
		SetLabelSettings(_labelSettings);

		//SetText(Text);
	}

	public new void SetText(string text)
	{
		base.SetText(text);

		for(int fontSize = _maxSize; fontSize >= _minSize; fontSize--)
		{
			_labelSettings.FontSize = fontSize;

			Vector2 size = _labelSettings.Font.GetStringSize(Text, HorizontalAlignment, -1, _labelSettings.FontSize);
			if(size.X <= Size.X && size.Y <= Size.Y)
			{
				break;
			}
		}
	}
}