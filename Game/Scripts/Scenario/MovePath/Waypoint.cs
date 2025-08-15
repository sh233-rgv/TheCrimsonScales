using Godot;
using GTweens.Easings;
using GTweensGodot.Extensions;

public partial class Waypoint : Node2D
{
	public Hex Hex { get; private set; }

	public void Init(Hex hex)
	{
		Hex = hex;

		Scale = Vector2.Zero;
		this.TweenScale(1f, 0.2f).SetEasing(Easing.OutBack).Play();
	}

	public void Destroy()
	{
		this.TweenScale(0f, 0.15f).SetEasing(Easing.InBack).OnComplete(QueueFree).Play();
	}
}