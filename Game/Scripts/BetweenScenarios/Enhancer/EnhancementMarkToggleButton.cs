using Godot;

public partial class EnhancementMarkToggleButton : ToggleButton<EnhancementMarkToggleButton>
{
	private Vector2 _normalizedPosition;

	public EnhancementMark EnhancementMark { get; private set; }

	public void Init(EnhancementMark enhancementMark)
	{
		base.Init();

		EnhancementMark = enhancementMark;

		Control parent = GetParent<Control>();
		_normalizedPosition = enhancementMark.NormalizedPosition;
		SetPosition(_normalizedPosition * parent.Size - 0.5f * Size);
	}
}