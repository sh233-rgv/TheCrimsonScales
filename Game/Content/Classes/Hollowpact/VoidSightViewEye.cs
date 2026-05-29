using Godot;

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
		_animationPlayer.Play("open");
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