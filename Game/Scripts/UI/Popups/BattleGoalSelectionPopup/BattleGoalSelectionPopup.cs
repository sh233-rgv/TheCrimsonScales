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
			_battleGoals.Add(battleGoal);
		}
	}
}