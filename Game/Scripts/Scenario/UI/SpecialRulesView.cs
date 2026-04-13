using Godot;

public partial class SpecialRulesView : Control
{
	[Export]
	private RichTextLabel _label;
	[Export]
	private Control _container;

	public RichTextParameters RichTextParameters { get; private set; }

	public override void _EnterTree()
	{
		base._EnterTree();

		RichTextParameters = _label.GetRichTextParameters();
	}

	public override void _Ready()
	{
		base._Ready();

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