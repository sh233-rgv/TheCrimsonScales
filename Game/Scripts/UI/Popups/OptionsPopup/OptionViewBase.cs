using Godot;

public abstract partial class OptionViewBase : Control
{
	public abstract void Init(OptionViewParameters parameters);

	public virtual void OnOpen()
	{
	}

	public abstract void Destroy();
}