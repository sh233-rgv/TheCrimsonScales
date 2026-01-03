public class SpiritualGainsPersonalQuest : TheCrimsonScalesPersonalQuest<PersonalQuestData>
{
	public override string Name => "Spiritual Gains";
	public override ClassModel ClassToUnlock => ModelDB.Class<HierophantModel>();
	public override int MaxProgress => 200;
	protected override int AtlasIndex => 11;
}