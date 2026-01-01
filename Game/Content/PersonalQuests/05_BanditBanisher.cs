public class BanditBanisher : TheCrimsonScalesPersonalQuest
{
	public override string Name => "Bandit Banisher";
	public override ClassModel ClassToUnlock => ModelDB.Class<ChainguardModel>();

	public override int MaxProgress => 10;

// 	public override ScenarioModel UnlockedScenarioModel => ModelDB.Scenario<Scenario035>();
	protected override int AtlasIndex => 5;
}