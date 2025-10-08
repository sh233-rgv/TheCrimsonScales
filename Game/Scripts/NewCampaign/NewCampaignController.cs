using Godot;
using GTweens.Builders;
using GTweensGodot.Extensions;

public partial class NewCampaignController : SceneController<NewCampaignController>
{
	[Export]
	private NewCampaignStep[] _steps;

	[Export]
	private ChoiceButton _backButton;
	[Export]
	private ChoiceButton _confirmButton;

	private NewCampaignSceneRequest _sceneRequest;

	private int _stepIndex;
	private NewCampaignStep _currentStep;

	public SavedCampaign Campaign { get; private set; }

	public override void _EnterTree()
	{
		_sceneRequest = AppController.Instance.SceneLoader.CurrentSceneRequest as NewCampaignSceneRequest;

		if(_sceneRequest == null)
		{
			_sceneRequest = new NewCampaignSceneRequest();
		}
	}

	public override void _Ready()
	{
		base._Ready();

		Campaign = SavedCampaign.New();

		_backButton.BetterButton.Pressed += OnBackPressed;
		_confirmButton.BetterButton.Pressed += OnConfirmPressed;

		AppController.Instance.AudioController.SetBGM("res://Audio/BGM/Call to Adventure FULL LOOP TomMusic.ogg");
		AppController.Instance.AudioController.SetBGS(null);

		SetStep(0);
	}

	public void AdvanceStep()
	{
		if(_stepIndex == _steps.Length - 1)
		{
			// Final step completed, time to start the campaign!
			AppController.Instance.SaveFile.SaveData.SavedCampaign = Campaign;

			AppController.Instance.SaveFile.Save();

			AppController.Instance.SceneLoader.RequestSceneChange(
				new BetweenScenariosSceneRequest(AppController.Instance.SaveFile.SaveData.SavedCampaign));

			return;
		}

		SetStep(_stepIndex + 1);
	}

	public void UpdateConfirmVisible()
	{
		_confirmButton.SetActive(_currentStep?.ConfirmButtonActive ?? false);
	}

	private void SetStep(int newStepIndex)
	{
		NewCampaignStep oldStep = _currentStep;

		_stepIndex = newStepIndex;
		_currentStep = _steps[_stepIndex];

		if(oldStep == null)
		{
			_currentStep.Activate();
		}
		else
		{
			GTweenSequenceBuilder.New()
				.AppendCallback(oldStep.Deactivate)
				.AppendCallback(UpdateConfirmVisible)
				.AppendTime(0.5f)
				.AppendCallback(_currentStep.Activate)
				.AppendCallback(UpdateConfirmVisible)
				.Build().Play();
		}
	}

	private void OnBackPressed()
	{
		if(_stepIndex == 0)
		{
			AppController.Instance.SceneLoader.RequestSceneChange(new MainMenuSceneRequest());
			return;
		}

		SetStep(_stepIndex - 1);
	}

	private void OnConfirmPressed()
	{
		AdvanceStep();
	}
}