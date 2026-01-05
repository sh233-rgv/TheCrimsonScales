using System.Collections.Generic;

public class WildStings : MirefootCardModel<WildStings.CardTop, WildStings.CardBottom>
{
	public override string Name => "Wild Stings";
	public override int Level => 7;
	public override int Initiative => 71;
	protected override int AtlasIndex => 23;

	public class CardTop : MirefootCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(1)
				.WithTargets(2)
				.WithConditions(Conditions.Poison1)
				.Build()),
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2)
				.WithTargets(2)
				.Build())
		];
	}

	public class CardBottom : MirefootCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SummonAbility.Builder()
				.WithSummonStats(new SummonStats()
				{
					Health = 3,
					Attack = 1,
					Traits =
					[
						new ApplyConditionTrait(Conditions.Poison1),
						new PerformOnDeathTrait(ConditionAbility.Builder().WithConditions(Conditions.Poison2)
							.WithTarget(Target.TargetAll | Target.Enemies).WithRange(1).Build())
					]
				})
				.WithName("Crypt Nettle")
				.WithTexturePath("res://Content/Classes/Mirefoot/CryptNettle.png")
				.WithGetValidHexes((abilityState, list) =>
					{
						RangeHelper.FindHexesInRange(abilityState.Performer.Hex, 3, true, list);

						list.RemoveAll(hex => !hex.HasHexObjectOfType<DifficultTerrain>() || !hex.IsUnoccupied());
					}
				)
				.Build())
		];

		protected override int XP => 1;
		protected override bool Persistent => true;
	}
}