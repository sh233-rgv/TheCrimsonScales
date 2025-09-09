using Godot;
using GTweens.Builders;
using GTweens.Easings;
using GTweens.Tweens;
using GTweensGodot.Extensions;

public partial class ScreenDistortion : FullScreenControl
{
	private static readonly StringName LensCenterName = "lens_center";
	private static readonly StringName LensRadiusName = "lens_radius";
	private static readonly StringName LensPowerName = "lens_power";
	private static readonly StringName CameraZoomName = "camera_zoom";

	private ShaderMaterial _shaderMaterial;
	private Node2D _target;

	public override void _Ready()
	{
		base._Ready();

		_shaderMaterial = (ShaderMaterial)Material;

		SetVisible(false);

		this.DelayedCall(() =>
		{
			SetTarget(GameController.Instance.CharacterManager.GetCharacter(0));

			SetPower(1f);
			SetRadius(0.4f);
			GTweenSequenceBuilder.New()
				// Disappear
				.Append(TweenPower(1.1f, 0.2f).SetEasing(Easing.OutCubic))
				.Join(TweenRadius(0.4f, 0.2f).SetEasing(Easing.OutCubic))
				.Append(TweenPower(0.4f, 0.5f).SetEasing(Easing.OutCubic))
				.Join(TweenRadius(0.3f, 0.5f).SetEasing(Easing.OutCubic))
				.Join(_target.TweenScale(0f, 0.5f).SetEasing(Easing.Linear))
				.Append(TweenPower(1f, 0.5f).SetEasing(Easing.OutBack))
				.Join(TweenRadius(0.4f, 0.2f).SetEasing(Easing.OutCubic))
				.AppendTime(2f)
				// Appear
				.Append(TweenPower(1.1f, 0.2f).SetEasing(Easing.OutCubic))
				.Join(TweenRadius(0.4f, 0.2f).SetEasing(Easing.OutCubic))
				.Append(TweenPower(0.4f, 0.5f).SetEasing(Easing.OutCubic))
				.Join(TweenRadius(0.3f, 0.5f).SetEasing(Easing.OutCubic))
				.Append(TweenPower(1f, 0.5f).SetEasing(Easing.OutBack))
				.Join(TweenRadius(0.4f, 0.2f).SetEasing(Easing.OutCubic))
				.Join(_target.TweenScale(1f, 0.2f).SetEasing(Easing.OutCubic))
				.AppendTime(2f)
				.Build().SetMaxLoops().Play();
		}, 0.1f);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if(_target == null)
		{
			return;
		}

		Camera2D camera = GameController.Instance.CameraController.Camera;
		Vector2 viewportPosition = _target.GetGlobalTransformWithCanvas().Origin / GetViewport().GetVisibleRect().Size;
		_shaderMaterial.SetShaderParameter(LensCenterName, viewportPosition);
		_shaderMaterial.SetShaderParameter(CameraZoomName, camera.Zoom.X);
	}

	public void SetTarget(Node2D target)
	{
		_target = target;
		SetVisible(_target != null);
	}

	public void SetPower(float power)
	{
		_shaderMaterial.SetShaderParameter(LensPowerName, power);
	}

	public void SetRadius(float radius)
	{
		_shaderMaterial.SetShaderParameter(LensRadiusName, radius);
	}

	public GTween TweenPower(float to, float duration)
	{
		return _shaderMaterial.TweenPropertyFloat(LensPowerName, to, duration);
	}

	public GTween TweenRadius(float to, float duration)
	{
		return _shaderMaterial.TweenPropertyFloat(LensRadiusName, to, duration);
	}
}