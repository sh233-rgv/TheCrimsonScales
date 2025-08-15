using Godot;

public partial class ConsumeElementEffectButton : EffectButton<ConsumeElementEffectButton.Parameters>
{
	public class Parameters : EffectButtonParameters
	{
		public override string ScenePath => "res://Scenes/Scenario/UI/EffectButtons/ConsumeElementEffectButton.tscn";

		public Element Element { get; }

		public Parameters(Element element)
		{
			Element = element;
		}
	}

	[Export]
	private TextureRect _textureRect;

	protected override void Init(Parameters parameters)
	{
		base.Init(parameters);

		_textureRect.Texture = ResourceLoader.Load<Texture2D>(Icons.GetElement(parameters.Element));
	}
}