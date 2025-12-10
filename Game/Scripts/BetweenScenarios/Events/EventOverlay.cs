using System;
using System.Collections.Generic;
using System.Threading;
using Fractural.Tasks;
using Godot;
using GTweensGodot.Extensions;

public partial class EventOverlay : Control
{
	[Export]
	private Control _background;
	[Export]
	private BetterButton _skipTextButton;

	[Export]
	private EventCard _eventCard;

	[Export]
	private PackedScene _eventChoiceButtonScene;
	[Export]
	private Control _eventChoiceButtonParent;

	private readonly List<EventChoiceButton> _choiceButtons = new List<EventChoiceButton>();

	private EventChoiceModel _chosenModel;

	public override void _Ready()
	{
		base._Ready();

		_skipTextButton.Pressed += OnSkipTextPressed;

		Hide();

		this.DelayedCall(() => Open(ModelDB.Event<City01>()), 2f);
	}

	public void Open(EventModel eventModel)
	{
		Sequence(eventModel).Forget();
	}

	private async GDTaskVoid Sequence(EventModel eventModel)
	{
		Show();

		_chosenModel = null;
		_skipTextButton.Show();

		_background.SetModulate(Colors.Transparent);
		_background.TweenModulateAlpha(1f, 0.3f).Play();

		CancellationToken cancellationToken = BetweenScenariosController.Instance.DestroyCancellationToken;

		foreach(EventChoiceModel choiceModel in eventModel.EventChoiceModels)
		{
			EventChoiceButton choiceButton = _eventChoiceButtonScene.Instantiate<EventChoiceButton>();
			_eventChoiceButtonParent.AddChild(choiceButton);
			choiceButton.Init(choiceModel, OnChoiceButtonPressed);
			_choiceButtons.Add(choiceButton);
		}

		await _eventCard.SetModelAndAnimate(eventModel, cancellationToken);

		_skipTextButton.Hide();

		foreach(EventChoiceButton choiceButton in _choiceButtons)
		{
			choiceButton.SetActive(true);
			await GDTask.Delay(0.3f, cancellationToken: cancellationToken);
		}

		await GDTask.WaitUntil(() => _chosenModel != null, cancellationToken: cancellationToken);

		foreach(EventChoiceButton choiceButton in _choiceButtons)
		{
			choiceButton.Disable();

			if(choiceButton.Model != _chosenModel)
			{
				choiceButton.SetActive(false);
			}
		}

		await _eventCard.Rotate(cancellationToken);
	}

	private void OnSkipTextPressed()
	{
		_eventCard.SkipText();
		_skipTextButton.Hide();
	}

	private void OnChoiceButtonPressed(EventChoiceButton choiceButton)
	{
		if(_chosenModel != null)
		{
			return;
		}

		_chosenModel = choiceButton.Model;
	}
}