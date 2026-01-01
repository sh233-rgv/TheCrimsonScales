public class HealthFirst : TheCrimsonScalesPersonalQuest
{
	public override string Name => "Health First";
	public override ClassModel ClassToUnlock => ModelDB.Class<StarslingerModel>();
	public override int MaxProgress => 7;
	protected override int AtlasIndex => 20;
}