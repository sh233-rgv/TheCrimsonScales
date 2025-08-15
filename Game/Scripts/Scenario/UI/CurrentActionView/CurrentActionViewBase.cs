using Godot;

public abstract partial class CurrentActionViewBase : Control
{
	public object Source { get; protected set; }

	public abstract void Init(CurrentActionViewParameters parameters);

	public abstract void Destroy();
}