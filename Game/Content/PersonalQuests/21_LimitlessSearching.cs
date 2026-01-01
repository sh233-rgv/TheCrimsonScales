public class LimitlessSearching : TheCrimsonScalesPersonalQuest
{
	public override string Name => "Limitless Searching";
	public override ClassModel ClassToUnlock => ModelDB.Class<StarslingerModel>();
	public override int MaxProgress => 30;
	protected override int AtlasIndex => 21;
}