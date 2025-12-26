using Godot;

public partial class CharacterTokenHexObjectEffectView : HexObjectEffectView<CharacterTokenHexObjectEffectView.Parameters>
{
	public class Parameters(Character owner) : HexObjectEffectViewParameters
	{
		public override string ScenePath => "res://Scenes/Scenario/HexObjectEffectViews/CharacterTokenHexObjectEffectView.tscn";

		public Character Owner { get; } = owner;
	}

	[Export]
	private Sprite2D _sprite;

	public override void Init(Parameters parameters)
	{
		base.Init(parameters);

		_sprite.SetTexture(parameters.Owner.ClassModel.CharacterTokenTexture);
	}
}