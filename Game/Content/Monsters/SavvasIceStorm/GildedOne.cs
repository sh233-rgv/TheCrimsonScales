using System.Linq;

public class GildedOne : SavvasIceStorm
{
	public override MonsterStats[] NamedLevelStats =>
		base.EliteLevelStats
			.Select(stats => stats with
			{
				Health = stats.Health * CharacterCount / 2,
				Traits = (stats.Traits ?? [])
				.Append(new ConditionImmunityTrait(Conditions.Disarm))
				.Append(new ConditionImmunityTrait(Conditions.Immobilize))
				.Append(new ForcedMovementImmunityTrait())
				.ToArray()
			})
			.ToArray();

	public override string Name => "Gilded One";
	public override MonsterModel ParentMonsterModel => ModelDB.Monster<SavvasIceStorm>();
}