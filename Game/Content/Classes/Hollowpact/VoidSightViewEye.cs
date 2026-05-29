using Godot;
using GTweens.Builders;
using GTweensGodot.Extensions;

public partial class VoidSightViewEye : Control
{
	[Export]
	private Sprite2D _pupil;
	[Export]
	private Node2D _pupilContainer;
	// [Export]
	// private Sprite2D _bottomEyelid;
	// [Export]
	// private Sprite2D _topEyelid;
	[Export]
	private AnimationPlayer _animationPlayer;

	public void Open()
	{
		_animationPlayer.Play("RESET");
		_animationPlayer.Play("open");
		float animationSpeed = AppController.Instance.DeviceOptions.GetTimeScale(TimeScale.Gameplay);
		_animationPlayer.SetSpeedScale(2f * animationSpeed);

		this.DelayedCall(Close, 4f / animationSpeed);
		// GTweenSequenceBuilder.New()
		// 	.AppendTime(5f / animationSpeed)
		// 	.AppendCallback(Close)
		// 	.Build().Play();
	}

	public void Close()
	{
		_animationPlayer.Play("close");
	}

	// public override void _Process(double delta)
	// {
	// 	base._Process(delta);
	//
	// 	float theta = GD.Randf() * Mathf.Tau;
	// 	Vector2 pointInCircle = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)) * Mathf.Sqrt(4 * GD.Randf());
	// 	_pupil.SetPosition(pointInCircle);
	// }
}