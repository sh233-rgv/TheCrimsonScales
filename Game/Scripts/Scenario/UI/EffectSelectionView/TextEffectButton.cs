using Godot;

public partial class TextEffectButton : EffectButton<TextEffectButton.Parameters>
{
	public class Parameters : EffectButtonParameters
	{
		public override string ScenePath => "res://Scenes/Scenario/UI/EffectButtons/TextEffectButton.tscn";

		public string Text { get; }

		public Parameters(string text)
		{
			Text = text;
		}
	}

	[Export]
	private RichTextLabel _label;

	protected override void Init(Parameters parameters)
	{
		base.Init(parameters);

		_label.SetText(parameters.Text);
	}
}