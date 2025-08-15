using Godot;
using GTweens.Builders;
using GTweens.Easings;
using GTweensGodot.Extensions;

public partial class ScenarioFlowchartArrow : Control
{
	[Export]
	private Control _container;
	[Export]
	private Control _linkContainer;

	public ScenarioButton From { get; private set; }
	public ScenarioButton To { get; private set; }
	public bool Linked { get; private set; }

	public void Init(ScenarioButton from, ScenarioButton to, bool linked)
	{
		From = from;
		To = to;
		Linked = linked;

		_linkContainer.SetVisible(linked);

		Vector2 fromCenter = from.GlobalPosition + from.Size * 0.5f * BetweenScenariosController.Instance.ScenarioFlowchart.GridScale;
		Vector2 toCenter = to.GlobalPosition + to.Size * 0.5f * BetweenScenariosController.Instance.ScenarioFlowchart.GridScale;

		float angle = (toCenter - fromCenter).Normalized().Angle();

		// Check if diagonal
		if(angle % (Mathf.Pi * 0.5f) > 0.1f)
		{
			_container.Size = new Vector2(_container.Size.X * 1.41f, _container.Size.Y);
			_container.Position = new Vector2(_container.Position.X * 1.41f, _container.Position.Y);
		}

		GlobalPosition = (fromCenter + toCenter) * 0.5f;
		Rotation = angle;

		//GlobalPosition = fromCenter;
	}

	public void AnimateIn()
	{
		SetVisible(true);
		//Modulate = new Color(1f, 1f, 1f, 0f);
		float sizeX = _container.Size.X;

		_container.Size = new Vector2(40f, _container.Size.Y);
		_container.Scale = new Vector2(0f, 1f);
		_linkContainer.Scale = Vector2.Zero;

		GTweenSequenceBuilder.New()
			//.Append(this.TweenModulateAlpha(1f, 0.2f))
			.Append(_container.TweenScaleX(1f, 0.1f))
			.Join(_container.TweenSizeX(sizeX, 0.45f).SetEasing(Easing.InOutSine))
			.Append(_linkContainer.TweenScale(Linked ? 1f : 0f, Linked ? 0.3f : 0f).SetEasing(Easing.OutBack))
			.Build().Play();
	}
}