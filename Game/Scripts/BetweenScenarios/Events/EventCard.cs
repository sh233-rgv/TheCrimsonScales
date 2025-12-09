using Godot;
using GTweens.Builders;
using GTweens.Easings;
using GTweensGodot.Extensions;

public partial class EventCard : Control
{
	private static readonly StringName RotationName = "y_rot";

	[Export]
	private Control _frontContainer;
	[Export]
	private ShapedEventText _frontEventText;

	[Export]
	private Control _backContainer;
	[Export]
	private ShapedEventText _backEventText;

	[Export]
	private SubViewportContainer _subViewportContainer;

	private ShaderMaterial _material;

	public override void _Ready()
	{
		base._Ready();

		_material = (ShaderMaterial)_subViewportContainer.Material;

		this.DelayedCall(() => SetModel(ModelDB.Event<City01>()));

		//this.DelayedCall(Rotate, 2f);
	}

	public void SetModel(EventModel eventModel)
	{
		_frontContainer.SetVisible(true);
		_backContainer.SetVisible(false);
		_frontEventText.SetModel(eventModel);
	}

	private void Rotate()
	{
		_frontContainer.SetVisible(true);
		_backContainer.SetVisible(false);
		GTweenSequenceBuilder.New()
			.Append(_material.TweenPropertyFloat(RotationName, 90f, 0.2f).SetEasing(Easing.Linear))
			.AppendCallback(() =>
			{
				_frontContainer.SetVisible(false);
				_backContainer.SetVisible(true);
			})
			.Append(_material.TweenPropertyFloat(RotationName, -90f, 0f))
			.Append(_material.TweenPropertyFloat(RotationName, 0f, 0.5f).SetEasing(Easing.OutBack))
			.Build().Play();
	}
}