public class CreaturesInTheNight : TheCrimsonScalesPersonalQuest
{
	public override string Name => "Creatures in the Night";
	public override ClassModel ClassToUnlock => ModelDB.Class<ChieftainModel>();
	public override int MaxProgress => 20;
	protected override int AtlasIndex => 6;
}