using System.Collections.Generic;
using Godot;

public partial class OptionsPopup : Popup<OptionsPopup.Request>
{
	public class Request : PopupRequest
	{
	}

	[Export]
	private BetterButton _confirmButton;

	[Export]
	private Control _optionViewParent;

	private readonly List<OptionViewBase> _options = new List<OptionViewBase>();

	public override void _Ready()
	{
		base._Ready();

		_confirmButton.Pressed += OnConfirmPressed;

		SavedOptions options = AppController.Instance.SaveFile.SaveData.Options;

		AddSliderOption("Music", options.BGMVolume);
		AddSliderOption("Ambience", options.BGSVolume);
		AddSliderOption("Sound Effects", options.SFXVolume);

		if(!Platform.DeskTop)
		{
			AddCheckmarkOption("Vibrations", options.VibrationsEnabled);
		}
	}

	protected override void OnOpen()
	{
		base.OnOpen();

		_confirmButton.SetEnabled(true, false);

		foreach(OptionViewBase option in _options)
		{
			option.OnOpen();
		}
	}

	protected override void OnClose()
	{
		base.OnClose();

		AppController.Instance.SaveFile.Save();
	}

	private void AddCheckmarkOption(string label, SavedOption<bool> option)
	{
		AddOption(new CheckmarkOptionView.Parameters(option, label));
	}

	private void AddSliderOption(string label, SavedOption<int> option)
	{
		AddOption(new SliderOptionView.Parameters(option, label));
	}

	private void AddOption(OptionViewParameters parameters)
	{
		PackedScene scene = ResourceLoader.Load<PackedScene>(parameters.ScenePath);
		OptionViewBase optionView = scene.Instantiate<OptionViewBase>();
		_optionViewParent.AddChild(optionView);
		optionView.Init(parameters);
		_options.Add(optionView);
	}

	private void OnConfirmPressed()
	{
		Close();
	}
}