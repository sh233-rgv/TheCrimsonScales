using System;
using Godot;

public partial class BattleGoalSelectionPopupBattleGoal : Control
{
	[Export]
	private BattleGoalToggleButton _battleGoalToggleButton;
	[Export]
	private BetterButton _betterButton;

	public BattleGoalModel BattleGoalModel => _battleGoalToggleButton.BattleGoalModel;

	public event Action<BattleGoalSelectionPopupBattleGoal> PressedEvent;

	public override void _Ready()
	{
		base._Ready();

		_betterButton.Pressed += OnPressed;
	}

	public void Init(BattleGoalModel battleGoalModel)
	{
		_betterButton.SetEnabled(false, false);
		_battleGoalToggleButton.Init(battleGoalModel);
	}

	public void SetSelected(bool active, bool canPress)
	{
		_battleGoalToggleButton.SetSelected(active, canPress);
		_betterButton.SetEnabled(canPress, false);
	}

	private void OnPressed()
	{
		PressedEvent?.Invoke(this);
	}
}