using System;
using System.Collections.Generic;
using Godot;

public partial class GoalsView : Control
{
	[Export]
	private PackedScene _goalScene;
	[Export]
	private Control _goalParent;

	private readonly List<GoalsViewGoal> _goals = new List<GoalsViewGoal>();

	public override void _Ready()
	{
		base._Ready();

		GameController.Instance.ScenarioModel.GoalAddedEvent += OnGoalAdded;
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		if(GameController.Instance != null && GameController.Instance.ScenarioModel != null)
		{
			GameController.Instance.ScenarioModel.GoalAddedEvent -= OnGoalAdded;
		}
	}

	// public void Update(List<ScenarioGoal> goals)
	// {
	// 	foreach(GoalsViewGoal goal in _goals)
	// 	{
	// 		goal.QueueFree();
	// 	}
	//
	// 	_goals.Clear();
	//
	// 	foreach(ScenarioGoal scenarioGoal in goals)
	// 	{
	// 		GoalsViewGoal goal = _goalScene.Instantiate<GoalsViewGoal>();
	// 		_goalParent.AddChild(goal);
	// 		goal.Init(scenarioGoal);
	// 		_goals.Add(goal);
	// 	}
	// }

	private void OnGoalAdded(ScenarioGoal scenarioGoal)
	{
		GoalsViewGoal goal = _goalScene.Instantiate<GoalsViewGoal>();
		_goalParent.AddChild(goal);
		goal.Init(scenarioGoal);
		_goals.Add(goal);
	}
}