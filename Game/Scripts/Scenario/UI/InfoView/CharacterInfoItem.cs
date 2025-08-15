using Godot;

public partial class CharacterInfoItem : FigureInfoItem<CharacterInfoItem.Parameters>
{
	public class Parameters(Character hexObject) : FigureInfoItemParameters(hexObject)
	{
		public override string ScenePath => "res://Scenes/Scenario/UI/InfoView/CharacterInfoItem.tscn";

		public Character Character { get; } = hexObject;
	}

	[Export]
	private Label _coinsLabel;
	[Export]
	private Label _xpLabel;

	private Character _character;

	public override void Init(Parameters parameters)
	{
		base.Init(parameters);

		_character = parameters.Character;

		_portraitTexture.SetTexture(_character.PortraitTexture);
		_portraitBorder.SetSelfModulate(_character.OutlineColor);

		UpdateCoins();
		UpdateXP();

		_character.CoinsChangedEvent += OnCoinsChanged;
		_character.XPChangedEvent += OnXPChanged;
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		if(_character != null)
		{
			_character.CoinsChangedEvent -= OnCoinsChanged;
			_character.XPChangedEvent -= OnXPChanged;
		}
	}

	private void UpdateCoins()
	{
		_coinsLabel.SetText(_character.ObtainedCoins.ToString());
	}

	private void UpdateXP()
	{
		_xpLabel.SetText(_character.ObtainedXP.ToString());
	}

	private void OnCoinsChanged(Character character)
	{
		UpdateCoins();
	}

	private void OnXPChanged(Character character)
	{
		UpdateXP();
	}
}