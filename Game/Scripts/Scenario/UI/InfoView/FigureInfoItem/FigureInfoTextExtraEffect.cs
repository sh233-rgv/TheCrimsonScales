using Godot;

public partial class FigureInfoTextExtraEffect : FigureInfoExtraEffect<FigureInfoTextExtraEffect.Parameters>
{
	public class Parameters : FigureInfoExtraEffectParameters
	{
		public override string ScenePath => "res://Scenes/Scenario/UI/InfoView/FigureEffects/FigureInfoTextExtraEffect.tscn";

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