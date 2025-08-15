using Godot;
using GTweens.Easings;
using GTweensGodot.Extensions;

public abstract partial class EffectInfoView<T> : EffectInfoViewBase
	where T : EffectInfoViewParameters
{
	private Control _parent;

	public sealed override void Init(Control parent, EffectInfoViewParameters parameters)
	{
		_parent = parent;

		Init((T)parameters);
	}

	public virtual void Init(T parameters)
	{
		//Scale = Vector2.Zero;
		Scale = Vector2.One * 0.5f;
		this.TweenScale(1f, 0.15f).SetEasing(Easing.OutBack).Play();

		this.DelayedCall(() =>
		{
			GlobalPosition = new Vector2(_parent.GlobalPosition.X + _parent.Size.X * 0.5f - this.Size.X * 0.5f, _parent.GlobalPosition.Y);
			PivotOffset = new Vector2(Size.X * 0.5f, Size.Y);
		});
	}

	public override void Destroy()
	{
		this.TweenScale(0.5f, 0.15f).SetEasing(Easing.InBack).OnComplete(QueueFree).Play();
	}
}