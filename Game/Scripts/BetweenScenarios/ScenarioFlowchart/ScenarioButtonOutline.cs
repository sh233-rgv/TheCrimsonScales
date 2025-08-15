using Godot;
using GTweens.Builders;
using GTweensGodot.Extensions;

public partial class ScenarioButtonOutline : Control
{
	[Export]
	private Panel _panel;

	[Export]
	private Panel[] _directionalExtensions;

	[Export]
	private Panel[] _diagonalExtensions;

	public void Init(ScenarioButton scenarioButton)
	{
		//GlobalPosition = scenarioButton.GlobalPosition;
		Position = scenarioButton.Position;

		Modulate = scenarioButton.Model.ScenarioChain.BaseScenarioChain.Color;
	}

	public void AnimateIn()
	{
		SetVisible(true);

		_panel.Scale = Vector2.Zero;

		StyleBoxFlat styleBox = (StyleBoxFlat)_panel.GetThemeStylebox("panel");
		styleBox = (StyleBoxFlat)styleBox.Duplicate();
		styleBox.SetCornerRadiusAll(60);

		_panel.AddThemeStyleboxOverride("panel", styleBox);

		GTweenSequenceBuilder.New()
			.Append(_panel.TweenScale(1f, 0.3f))
			.Join(CustomGTweenExtensions.Tween(
				t => styleBox.SetCornerRadiusAll(Mathf.RoundToInt(Mathf.Lerp(60, 30, t))), 0.3f))
			.Build().Play();
	}

	public void AnimateDirectionalExtension(int directionIndex, bool instantly = false)
	{
		Panel directionalExtension = _directionalExtensions[directionIndex];

		GTweenSequenceBuilder.New()
			.Append(directionalExtension.TweenSizeX(250, 0.5f))
			.Build().Play(instantly);
	}

	public void AnimateDiagonalExtension(int diagonalIndex, bool instantly = false)
	{
		Panel diagonalExtension = _diagonalExtensions[diagonalIndex];

		GTweenSequenceBuilder.New()
			.Append(diagonalExtension.TweenSizeX(250, 0.5f))
			.Join(diagonalExtension.TweenSizeY(250, 0.5f))
			.Build().Play(instantly);
	}
}