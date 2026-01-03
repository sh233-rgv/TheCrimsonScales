public class FieldResearch : TheCrimsonScalesPersonalQuest<PersonalQuestData>
{
	public override string Name => "Field Research";
	public override ClassModel ClassToUnlock => ModelDB.Class<MirefootModel>();
	public override int MaxProgress => 20;
	protected override int AtlasIndex => 17;
}