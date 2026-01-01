public class AnAdderDivides : TheCrimsonScalesPersonalQuest
{
	public override string Name => "An Adder Divides";
	public override ClassModel ClassToUnlock => ModelDB.Class<MirefootModel>();
	public override int MaxProgress => 6;
	protected override int AtlasIndex => 16;
}