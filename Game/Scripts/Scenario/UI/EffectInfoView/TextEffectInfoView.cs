using Godot;

public partial class TextEffectInfoView : EffectInfoView<TextEffectInfoView.Parameters>
{
	public class Parameters : EffectInfoViewParameters
	{
		public override string ScenePath => "res://Scenes/Scenario/UI/EffectInfoViews/TextEffectInfoView.tscn";

		public string Text { get; }

		public Parameters(string text)
		{
			Text = text;
		}
	}

	[Export]
	private RichTextLabel _richTextLabel;

	public override void Init(Parameters parameters)
	{
		base.Init(parameters);

		_richTextLabel.SetText(parameters.Text);
	}
}