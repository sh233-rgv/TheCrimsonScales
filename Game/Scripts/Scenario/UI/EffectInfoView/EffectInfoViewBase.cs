using Godot;

public abstract partial class EffectInfoViewBase : Control
{
	public abstract void Init(Control parent, EffectInfoViewParameters parameters);

	public abstract void Destroy();
}