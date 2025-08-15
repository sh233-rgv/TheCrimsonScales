using Godot;

[GlobalClass]
public partial class AnimatedSpriteSheet2D : Sprite2D
{
	[Export]
	private int _frameCount;
	[Export]
	private int _framesPerSecond;

	private float _frame;

	public override void _Process(double delta)
	{
		base._Process(delta);

		_frame += (float)delta * _framesPerSecond;
		_frame %= _frameCount;
		Frame = Mathf.FloorToInt(_frame);
	}
}