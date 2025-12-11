using System.Collections.Generic;
using System.Threading;
using Fractural.Tasks;
using Godot;
using GTweens.Easings;
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
	private ChoiceButton _continueButton;

	[Export]
	private PackedScene _eventChoiceButtonScene;
	[Export]
	private Control _eventChoiceButtonParent;

	private readonly List<EventChoiceButton> _choiceButtons = new List<EventChoiceButton>();

	private EventChoiceModel _chosenModel;
	private bool _continuePressed;

	public override void _Ready()
	{
		base._Ready();

		_skipTextButton.Pressed += OnSkipTextPressed;
		_continueButton.BetterButton.Pressed += OnContinuePressed;

		Hide();
	}

	public void Open(EventType eventType)
	{
		DrawEventCard(eventType).Forget();
	}

	private async GDTaskVoid DrawEventCard(EventType eventType)
	{
		AppController.Instance.SaveFile.BlockSaving(this);

		EventModel eventModel;
		if(eventType == EventType.City)
		{
			eventModel = BetweenScenariosController.Instance.SavedCampaign.SavedEvents.DrawCityEvent();
		}
		else
		{
			eventModel = BetweenScenariosController.Instance.SavedCampaign.SavedEvents.DrawRoadEvent();
		}

		Show();

		_chosenModel = null;
		_continuePressed = false;
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

		// Initialize state
		SavedEventState savedEventState = new SavedEventState(_chosenModel);
		_chosenModel!.InitState(savedEventState, BetweenScenariosController.Instance.SavedCampaign);

		await _eventCard.Rotate(_chosenModel.GetStoryText(savedEventState), cancellationToken);

		_skipTextButton.Show();
		await _eventCard.AnimateBackText(cancellationToken: cancellationToken);

		foreach(EventChoiceButton choiceButton in _choiceButtons)
		{
			choiceButton.SetActive(false);
		}

		await GDTask.Delay(0.3f, cancellationToken: cancellationToken);

		_continueButton.SetActive(true);

		await GDTask.WaitUntil(() => _continuePressed, cancellationToken: cancellationToken);

		_continueButton.SetActive(false);

		bool hasNonImmediateReward = false;
		List<EventReward> rewards = _chosenModel.GetRewards(savedEventState);
		foreach(EventReward reward in rewards)
		{
			if(reward.Type == EventRewardType.Immediate)
			{
				await reward.Resolve();
			}
			else
			{
				hasNonImmediateReward = true;
			}
		}

		if(hasNonImmediateReward)
		{
			BetweenScenariosController.Instance.SavedCampaign.SavedEvents.AddSavedEventState(savedEventState);
		}

		EventResolveType eventResolveType = _chosenModel.GetEventResolveType(savedEventState);
		if(eventResolveType == EventResolveType.ReturnCardToBottom)
		{
			if(eventModel.EventType == EventType.City)
			{
				BetweenScenariosController.Instance.SavedCampaign.SavedEvents.ReturnCityEventToBottom(eventModel);
			}
			else
			{
				BetweenScenariosController.Instance.SavedCampaign.SavedEvents.ReturnRoadEventToBottom(eventModel);
			}
		}

		AppController.Instance.SaveFile.UnblockSaving(this);
		AppController.Instance.SaveFile.Save();

		_background.TweenModulateAlpha(0f, 0.3f).Play();
		await _eventCard.TweenScale(0f, 0.3f).SetEasing(Easing.InBack).PlayAsync(cancellationToken: cancellationToken);

		Hide();
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

	private void OnContinuePressed()
	{
		_continuePressed = true;
	}
}