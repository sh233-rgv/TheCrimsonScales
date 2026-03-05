using System.Linq;

public class Echo : LivingSpirit
{
	public override MonsterStats[] NamedLevelStats =>
		base.EliteLevelStats
			.Select(stats => stats with
			{
				Health = 6 * CharacterCount,
				Traits = (stats.Traits ?? [])
				.Append(new AllDamageImmunityTrait())
				.Append(new AllNegativeConditionImmunityTrait())
				.Append(new PermanentConditionTrait(Conditions.Invisible))
				.ToArray()
			})
			.ToArray();

	public override string Name => "Echo";
	public override MonsterModel ParentMonsterModel => ModelDB.Monster<LivingSpirit>();
}