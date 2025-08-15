using Godot;

public partial class SpecialRulesView : Control
{
	[Export]
	private RichTextLabel _label;
	[Export]
	private Control _container;

	private Font _font;

	public override void _Ready()
	{
		base._Ready();

		_font = _label.GetThemeFont("normal_font");

		Hide();
	}

	public void SetText(string text)
	{
		Show();

		_label.SetText(text);

		this.DelayedCall(() =>
		{
			float textSize = _label.GetContentHeight();
			float containerSize = Mathf.Clamp(textSize + 100f, 160f, 300f);
			_container.SetSize(new Vector2(_container.Size.X, containerSize));
		});
	}

	public void RemoveText()
	{
		Hide();
	}
}