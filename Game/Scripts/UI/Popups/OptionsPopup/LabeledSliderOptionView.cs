using Godot;

public partial class LabeledSliderOptionView : OptionView<LabeledSliderOptionView.Parameters, int>
{
	public class Parameters : OptionViewParameters<int>
	{
		public LabeledOptions LabeledOptions { get; }

		public override string ScenePath => "res://Scenes/UI/Popups/OptionsPopup/LabeledSliderOptionView.tscn";

		public Parameters(SavedOption<int> savedOption, string label, LabeledOptions labeledOptions)
			: base(savedOption, label)
		{
			LabeledOptions = labeledOptions;
		}
	}

	[Export]
	private BetterSlider _slider;
	[Export]
	private Label _valueLabel;

	private bool _valueSet;

	public override void _Ready()
	{
		base._Ready();

		_slider.ValueChangedEvent += OnSliderValueChanged;
	}

	public override void OnOpen()
	{
		base.OnOpen();

		if(_valueSet)
		{
			return;
		}

		_valueSet = true;

		this.DelayedCall(() =>
		{
			_slider.SetRange(0, _parameters.LabeledOptions.OptionCount - 1);
			_slider.SetValue(SavedOption.Value);
		});
	}

	protected override void OnValueChanged(int value)
	{
		base.OnValueChanged(value);

		LabeledOption option = _parameters.LabeledOptions.GetOption(value);
		_valueLabel.SetText(option.Label);
		// string difficultyName = DifficultyToString(value);
		//
		// _valueLabel.Text = $"{difficultyName} ({(value >= 0 ? "+" : string.Empty)}{value})";
	}

	private void OnSliderValueChanged(float value)
	{
		SavedOption.SetValue(Mathf.RoundToInt(value));
	}
}