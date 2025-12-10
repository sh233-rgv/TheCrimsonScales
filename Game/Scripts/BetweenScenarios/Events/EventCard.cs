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

		SetScale(Vector2.One * 0.001f);

		await GDTask.Yield(cancellationToken);

		_frontContainer.SetVisible(true);
		_backContainer.SetVisible(true);
		_frontContainer.SetModulate(Colors.White);
		_backContainer.SetModulate(Colors.Transparent);

		_frontEventText.SetText(eventModel.Text, false);

		await GDTask.Yield(cancellationToken);
		await GDTask.Delay(0.2f, cancellationToken: cancellationToken);

		SetPivotOffset(Size * 0.5f);
		await this.TweenScale(1f, 0.6f).SetEasing(Easing.OutBack).PlayAsync(cancellationToken);

		await AnimateText(_frontEventText, cancellationToken);
	}

	private async GDTask AnimateText(ShapedEventText shapedEventText, CancellationToken cancellationToken)
	{
		const float charactersPerSecond = 50f;
		float charactersToDisplay = 0f;
		bool waitedFrame = false;

		foreach(RichTextLabel label in shapedEventText.RichTextLabels)
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

	public async GDTask Rotate(string storyText, CancellationToken cancellationToken)
	{
		_skipText = false;

		_backEventText.SetText(storyText, false);

		await GDTask.Yield(cancellationToken);
		await GDTask.Delay(0.2f, cancellationToken: cancellationToken);

		await GTweenSequenceBuilder.New()
			.Append(_material.TweenPropertyFloat(RotationName, 90f, 0.2f).SetEasing(Easing.Linear))
			.AppendCallback(() =>
			{
				_frontContainer.SetModulate(Colors.Transparent);
				_backContainer.SetModulate(Colors.White);
			})
			.Append(_material.TweenPropertyFloat(RotationName, -90f, 0f))
			.Append(_material.TweenPropertyFloat(RotationName, 0f, 0.5f).SetEasing(Easing.OutBack))
			.Build().PlayAsync(cancellationToken);
	}

	public async GDTask AnimateBackText(CancellationToken cancellationToken)
	{
		await AnimateText(_backEventText, cancellationToken);
	}

	public void SkipText()
	{
		_skipText = true;
	}
}