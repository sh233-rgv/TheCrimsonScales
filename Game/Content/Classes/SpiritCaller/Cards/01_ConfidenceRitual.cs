using System.Collections.Generic;

public class ConfidenceRitual : SpiritCallerCardModel<ConfidenceRitual.CardTop, ConfidenceRitual.CardBottom>
{
	public override string Name => "Confidence Ritual";
	public override int Level => 1;
	public override int Initiative => 30;
	protected override int AtlasIndex => 28 - 1;

	public class CardTop : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(new DynamicInt<AttackAbility.State>(state =>
					3 + (state.Performer is Character characterOwner ? Spirit.GetSpirits(characterOwner).Count : 0)))
				.WithRange(2)
				.Build()),
		];
	}

	public class CardBottom : SpiritCallerCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			//new AbilityCardAbility(OtherActiveAbility.Builder().WithOnActivate())
			//TODO
		];
	}
}