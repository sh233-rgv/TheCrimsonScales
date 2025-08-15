using Godot;
using GTweens.Builders;
using GTweensGodot.Extensions;

public partial class CharacterStartHexMoveIndicator : Control
{
	[Export]
	private Control _icon;
	[Export]
	private Line2D _line2D;

	private bool _opened;

	private Character _character;

	public override void _Ready()
	{
		base._Ready();

		float duration = 0.8f;
		float rotationDuration = 0.4f;
		float maxRotation = 5f;
		_icon.RotationDegrees = maxRotation;

		GTweenSequenceBuilder.New()
			.Append(_icon.TweenRotationDegrees(-maxRotation, rotationDuration))
			.Append(_icon.TweenRotationDegrees(maxRotation, rotationDuration))
			.Build().SetMaxLoops().Play();

		GTweenSequenceBuilder.New()
			.Append(_icon.TweenScale(1.1f, duration))
			.Append(_icon.TweenScale(1f, duration))
			.Build().SetMaxLoops().Play();

		this.TweenModulateAlpha(0f, 0f).Play();

		SetProcess(false);
	}

	public void Open(Character character)
	{
		if(_opened)
		{
			return;
		}

		_opened = true;

		_character = character;

		this.TweenModulateAlpha(1f, 0.1f).Play();

		SetProcess(true);
	}

	public void Close()
	{
		if(!_opened)
		{
			return;
		}

		_opened = false;

		this.TweenModulateAlpha(0f, 0.1f).Play();

		SetProcess(false);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if(_character != null)
		{
			_line2D.SetPointPosition(0, _character.GetGlobalTransformWithCanvas().Origin);
		}
	}

	public void Update(Vector2 currentPosition)
	{
		_icon.Position = currentPosition;

		_line2D.SetPointPosition(0, _character.GetGlobalTransformWithCanvas().Origin);
		_line2D.SetPointPosition(1, currentPosition);
	}
}