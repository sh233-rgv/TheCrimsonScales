public class ProtectAndServe : TheCrimsonScalesPersonalQuest
{
	public override string Name => "Protect and Serve";
	public override ClassModel ClassToUnlock => ModelDB.Class<BombardModel>();
	public override int MaxProgress => 10;
	protected override int AtlasIndex => 0;
}