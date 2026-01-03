public class MutualSupport : TheCrimsonScalesPersonalQuest<PersonalQuestData>
{
	public override string Name => "Mutual Support";
	public override ClassModel ClassToUnlock => ModelDB.Class<FireKnightModel>();
	public override int MaxProgress => 30;
	protected override int AtlasIndex => 9;
}