using Godot;

public partial class ScreenDistortion : FullScreenControl
{
	private static readonly StringName LensCenterName = "lens_center";
	private static readonly StringName LensRadiusName = "lens_radius";
	private static readonly StringName LensPowerName = "lens_power";
	
	private ShaderMaterial _shaderMaterial;
	private Node2D _target;

	public override void _Ready()
	{
		base._Ready();

		_shaderMaterial = (ShaderMaterial)Material;

		SetVisible(false);

		// this.DelayedCall(() =>
		// {
		// 	SetTarget(GameController.Instance.CharacterManager.GetCharacter(0));
		// }, 0.1f);
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
		_shaderMaterial.SetShaderParameter(LensRadiusName, camera.Zoom.X * 0.8f);
	}

	public void SetTarget(Node2D target)
	{
		_target = target;
		SetVisible(_target != null);
	}
}