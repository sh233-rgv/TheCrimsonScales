using Fractural.Tasks;
using Godot;

public partial class ScreenDistortion : FullScreenControl
{
	private ShaderMaterial _shaderMaterial;
	
	public override void _Ready()
	{
		base._Ready();

		_shaderMaterial = (ShaderMaterial)Material;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		// Get mouse position in viewport
		Vector2 mousePos = GetViewport().GetMousePosition();

		// Convert to normalized UV coordinates (0..1)
		Vector2 uv = new Vector2(
			mousePos.X / GetViewport().GetVisibleRect().Size.X,
			mousePos.Y / GetViewport().GetVisibleRect().Size.Y
		);

		_shaderMaterial.SetShaderParameter("lens_center", uv);
	}
}