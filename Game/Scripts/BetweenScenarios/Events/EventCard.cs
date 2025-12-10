using System.Threading;
using Fractural.Tasks;
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

	[Export]
	private Label _numberLabel;

	private ShaderMaterial _material;

	private bool _skipText;

	public override void _Ready()
	{
		base._Ready();

		_material = (ShaderMaterial)_subViewportContainer.Material;
	}

	public async GDTask SetModelAndAnimate(EventModel eventModel, CancellationToken cancellationToken)
	{
		_skipText = false;
		_numberLabel.SetText(eventModel.Number.ToString());

		SetScale(Vector2.Zero);

		await GDTask.Yield(cancellationToken);

		_frontContainer.SetVisible(true);
		_backContainer.SetVisible(false);
		_frontEventText.SetModel(eventModel, false);

		await GDTask.Yield(cancellationToken);
		await GDTask.Delay(0.3f, cancellationToken: cancellationToken);

		SetPivotOffset(Size * 0.5f);
		await this.TweenScale(1f, 0.6f).SetEasing(Easing.OutBack).PlayAsync(cancellationToken);

		const float charactersPerSecond = 50f;
		float charactersToDisplay = 0f;
		bool waitedFrame = false;
		foreach(RichTextLabel label in _frontEventText.RichTextLabels)
		{
			int labelLength = label.Text.Length;
			while(true)
			{
				if(_skipText)
				{
					charactersToDisplay += Mathf.Inf;
				}

				if(waitedFrame)
				{
					charactersToDisplay += charactersPerSecond * (float)GetProcessDeltaTime();
					waitedFrame = false;
				}

				label.SetVisibleCharacters(Mathf.Min(Mathf.FloorToInt(charactersToDisplay), labelLength));

				if(charactersToDisplay > labelLength)
				{
					charactersToDisplay -= labelLength;
					break;
				}

				await GDTask.Yield(cancellationToken);
				waitedFrame = true;
			}
		}
	}

	// private async GDTask AnimateText()
	// {
	// 	
	// }

	public void SkipText()
	{
		_skipText = true;
	}

	public async GDTask Rotate(CancellationToken cancellationToken)
	{
		_frontContainer.SetVisible(true);
		_backContainer.SetVisible(false);
		await GTweenSequenceBuilder.New()
			.Append(_material.TweenPropertyFloat(RotationName, 90f, 0.2f).SetEasing(Easing.Linear))
			.AppendCallback(() =>
			{
				_frontContainer.SetVisible(false);
				_backContainer.SetVisible(true);
			})
			.Append(_material.TweenPropertyFloat(RotationName, -90f, 0f))
			.Append(_material.TweenPropertyFloat(RotationName, 0f, 0.5f).SetEasing(Easing.OutBack))
			.Build().PlayAsync(cancellationToken);
	}
}