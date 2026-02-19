using System.Collections.Generic;
using Godot;

public partial class BattleGoalSelectionPopup : Popup<BattleGoalSelectionPopup.Request>
{
	public class Request : PopupRequest
	{
		public Character Character { get; init; }
	}

	[Export]
	private PackedScene _battleGoalScene;
	[Export]
	private Control _battleGoalsParent;

	private readonly List<BattleGoalSelectionPopupBattleGoal> _battleGoals = new List<BattleGoalSelectionPopupBattleGoal>();

	protected override void OnOpen()
	{
		base.OnOpen();

		foreach(BattleGoalModel battleGoalModel in PopupRequest.Character.AvailableBattleGoals)
		{
			BattleGoalSelectionPopupBattleGoal battleGoal = _battleGoalScene.Instantiate<BattleGoalSelectionPopupBattleGoal>();
			_battleGoalsParent.AddChild(battleGoal);
			battleGoal.Init(battleGoalModel);
			battleGoal.PressedEvent += OnBattleGoalPressed;
			_battleGoals.Add(battleGoal);

			if(PopupRequest.Character.BattleGoal != null)
			{
				battleGoal.SetSelected(battleGoal.BattleGoalModel == PopupRequest.Character.BattleGoal);
			}
		}
	}

	protected override void OnClosed()
	{
		base.OnClosed();

		foreach(BattleGoalSelectionPopupBattleGoal battleGoal in _battleGoals)
		{
			battleGoal.QueueFree();
		}

		_battleGoals.Clear();
	}

	private void OnBattleGoalPressed(BattleGoalSelectionPopupBattleGoal battleGoal)
	{
		if(battleGoal.BattleGoalModel == PopupRequest.Character.BattleGoal)
		{
			return;
		}

		PopupRequest.Character.SetBattleGoal(battleGoal.BattleGoalModel);

		foreach(BattleGoalSelectionPopupBattleGoal otherBattleGoal in _battleGoals)
		{
			otherBattleGoal.SetSelected(otherBattleGoal == battleGoal);
		}
	}
}