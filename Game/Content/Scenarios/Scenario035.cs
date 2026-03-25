using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class Scenario035 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario035.tscn";
	public override int ScenarioNumber => 35;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<PersonalQuestScenarioChain>();
	public override IEnumerable<ScenarioConnection> Connections => [new ScenarioConnection<Scenario036>()];

	protected override ScenarioGoals CreateScenarioGoals() =>
		new KillAllEnemiesScenarioGoals(customText: "Kill all enemies and keep the Chainguard alive to win this scenario.");

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		NPC chainguard = await SpawnNPC(GameController.Instance.Map.GetMarker(Marker.Type.a).Hex, 6 + ScenarioLevel * 3, "Chainguard",
			"res://Content/Classes/Chainguard", 50, [
				MoveAbility.Builder().WithDistance(3).Build(),
				AttackAbility.Builder().WithDamage(2).WithPush(2).Build()
			], $"{Icons.Inline(Icons.Move)}3\n{Icons.Inline(Icons.Attack)}2, {Icons.Inline(Icons.Push)}2");


		GameController.Instance.Map.Treasures.First(treasure => treasure.TreasureNumber == 28).SetItemLoot(ModelDB.Item<ChainMace>());
		GameController.Instance.Map.Treasures.First(treasure => treasure.TreasureNumber == 40).SetItemLoot(AbilityCmd.GetRandomAvailableStone());
		
		ScenarioEvents.FigureKilledEvent.Subscribe(this,
			parameters => parameters.Figure == chainguard,
			async _ =>
			{
				await AbilityCmd.Lose();
			});
		
		UpdateScenarioText($"""
		                   The Chainguard is an ally to you and an enemy to all monsters. He acts on initiative 50 each round, performing "{Icons.Inline(Icons.Move)}3, {Icons.Inline(Icons.Attack)}2, {Icons.Inline(Icons.Push)}2".
		                   The Chainguard draws from the monster ability deck.
		                   
		                   If the Chainguard is killed, the scenario is lost.
		                   """);
	}
}