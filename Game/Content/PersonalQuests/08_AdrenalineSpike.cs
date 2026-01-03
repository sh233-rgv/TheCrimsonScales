public class AdrenalineSpike : TheCrimsonScalesPersonalQuest<PersonalQuestData>
{
	public override string Name => "Adrenaline Spike";
	public override ClassModel ClassToUnlock => ModelDB.Class<FireKnightModel>();
	public override int MaxProgress => 15;
	protected override int AtlasIndex => 8;
}