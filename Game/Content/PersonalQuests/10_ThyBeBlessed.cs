public class ThyBeBlessed : TheCrimsonScalesPersonalQuest<PersonalQuestData>
{
	public override string Name => "Thy be Blessed";
	public override ClassModel ClassToUnlock => ModelDB.Class<HierophantModel>();
	public override int MaxProgress => 12;
	protected override int AtlasIndex => 10;
}