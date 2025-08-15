using Godot;
using GTweens.Easings;

public abstract partial class CurrentActionView<T> : CurrentActionViewBase
	where T : CurrentActionViewParameters
{
	public sealed override void Init(CurrentActionViewParameters parameters)
	{
		Init((T)parameters);
	}

	public virtual void Init(T parameters)
	{
		Source = parameters.Source;

		Scale = Vector2.Zero;

		this.DelayedCall(() =>
		{
			this.TweenScale(1f, 0.15f).SetEasing(Easing.OutBack).PlayFastForwardable();

			this.DelayedCall(() =>
			{
				PivotOffset = new Vector2(Size.X * 0.5f, Size.Y * 0.5f);
			});
		});
	}

	public override void Destroy()
	{
		if(Scale != Vector2.One)
		{
			QueueFree();
		}
		else
		{
			this.TweenScale(0f, 0.1f).OnComplete(QueueFree).PlayFastForwardable();
		}
	}
}