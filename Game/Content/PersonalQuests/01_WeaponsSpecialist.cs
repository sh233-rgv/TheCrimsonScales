public class WeaponsSpecialist : TheCrimsonScalesPersonalQuest
{
	public override string Name => "Weapons Specialist";
	public override ClassModel ClassToUnlock => ModelDB.Class<BombardModel>();
	public override int MaxProgress => 15;
	protected override int AtlasIndex => 1;
}