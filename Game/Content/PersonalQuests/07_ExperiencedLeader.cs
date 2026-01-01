public class ExperiencedLeader : TheCrimsonScalesPersonalQuest
{
	public override string Name => "Experienced Leader";
	public override ClassModel ClassToUnlock => ModelDB.Class<ChieftainModel>();
	public override int MaxProgress => 12;
	protected override int AtlasIndex => 7;
}