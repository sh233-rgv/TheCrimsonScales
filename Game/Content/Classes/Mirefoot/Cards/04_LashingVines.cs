using System.Collections.Generic;

public class LashingVines : MirefootCardModel<LashingVines.CardTop, LashingVines.CardBottom>
{
	public override string Name => "Lashing Vines";
	public override int Level => 1;
	public override int Initiative => 13;
	protected override int AtlasIndex => 4;

	public class CardTop : MirefootCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder().WithDamage(0).Build()),
			new AbilityCardAbility(AttackAbility.Builder().WithDamage(0).Build()),
			new AbilityCardAbility(AttackAbility.Builder().WithDamage(0).Build())
		];
	}

	public class CardBottom : MirefootCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SummonAbility.Builder()
				.WithSummonStats(new SummonStats()
				{
					Health = 1,
					Attack = 1,
					Traits = [new TargetsTrait(3)]
				})
				.WithName("Flailing Ivies")
				.WithTexturePath("res://Content/Classes/Mirefoot/FlailingIvies.png")
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