public class AnAdderDivides : TheCrimsonScalesPersonalQuest<PersonalQuestData>
{
	public override string Name => "An Adder Divides";
	public override ClassModel ClassToUnlock => ModelDB.Class<MirefootModel>();

	public override int MaxProgress => 6;

	//public override ScenarioModel UnlockedScenarioModel => ModelDB.Scenario<Scenario039>();
	protected override int AtlasIndex => 16;
}