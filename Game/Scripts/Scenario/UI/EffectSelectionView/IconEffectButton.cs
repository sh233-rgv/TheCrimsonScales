using Godot;

public partial class IconEffectButton : EffectButton<IconEffectButton.Parameters>
{
	public class Parameters : EffectButtonParameters
	{
		public override string ScenePath => "res://Scenes/Scenario/UI/EffectButtons/IconEffectButton.tscn";

		public string IconPath { get; }

		public Parameters(string iconPath)
		{
			IconPath = iconPath;
		}
	}

	[Export]
	private TextureRect _textureRect;

	protected override void Init(Parameters parameters)
	{
		base.Init(parameters);

		_textureRect.SetTexture(ResourceLoader.Load<Texture2D>(parameters.IconPath));
	}
}