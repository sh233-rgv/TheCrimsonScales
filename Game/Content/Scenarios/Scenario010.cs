using System.Collections.Generic;
using Fractural.Tasks;

public class Scenario010 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario010.tscn";
	public override int ScenarioNumber => 10;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<WondrousScenarioChain>();
	//public override IEnumerable<ScenarioConnection> Connections => [new ScenarioConnection<Scenario002>(true)];

	protected override ScenarioGoals CreateScenarioGoals() => new KillAlLEnemiesScenarioGoals();

	public override string BGSPath => "res://Audio/BGS/Forest Day.ogg";

	public override async GDTask StartBeforeFirstRoomRevealed()
	{
		await base.StartBeforeFirstRoomRevealed();

		List<Objective> objectives = GameController.Instance.Map.GetChildrenOfType<Objective>();
		int objectiveHealth =
			2 * (GameController.Instance.SavedCampaign.Characters.Count + GameController.Instance.SavedScenario.ScenarioLevel + 1);
		foreach(Objective objective in objectives)
		{
			objective.Init(objectiveHealth, "Supply Crate");
		}

		GameController.Instance.Map.Treasures[0].SetItemLoot(AbilityCmd.GetRandomAvailableStone());
	}
}