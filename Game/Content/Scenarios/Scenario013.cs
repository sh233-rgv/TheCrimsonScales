using Fractural.Tasks;

public class Scenario013 : ScenarioModel
{
	public override string ScenePath => "res://Content/Scenarios/Scenario013.tscn";
	public override int ScenarioNumber => 13;
	public override ScenarioChain ScenarioChain => ModelDB.ScenarioChain<MainCampaignScenarioChain>();

	protected override ScenarioGoals CreateScenarioGoals() => new KillAlLEnemiesScenarioGoals();

	public override string BGSPath => "res://Audio/BGS/Cave.ogg";

	public override async GDTask StartAfterFirstRoomRevealed()
	{
		await base.StartAfterFirstRoomRevealed();

		GameController.Instance.Map.Treasures[0].SetItemLoot(AbilityCmd.GetRandomAvailableStone());

		foreach(Character character in GameController.Instance.CharacterManager.Characters)
		{
			await AbilityCmd.AddCondition(null, character, Conditions.Poison1);
		}

		ScenarioEvents.AbilityStartedEvent.Subscribe(this,
			parameters =>
				parameters.AbilityState is AttackAbility.State &&
				parameters.Performer is Monster monster &&
				monster.MonsterModel == ModelDB.Monster<LivingCorpse>(),
			async parameters =>
			{
				AttackAbility.State attackAbilityState = (AttackAbility.State)parameters.AbilityState;
				attackAbilityState.AdjustTargets(1);

				await GDTask.CompletedTask;
			}
		);
	}
}