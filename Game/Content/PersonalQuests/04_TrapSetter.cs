public class TrapSetter : TheCrimsonScalesPersonalQuest<PersonalQuestData>
{
	public override string Name => "Trap Setter";
	public override ClassModel ClassToUnlock => ModelDB.Class<ChainguardModel>();
	public override int MaxProgress => 15;
	protected override int AtlasIndex => 4;
}