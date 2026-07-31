using System;
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class SpikedEmbrace : ThornreaperCardModel<SpikedEmbrace.CardTop, SpikedEmbrace.CardBottom>
{
	public override string Name => "Spiked Embrace";
	public override int Level => 1;
	public override int Initiative => 31;
	protected override int AtlasIndex => 12;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2, new AttackSquare(this, new Vector2(0.28231278f, 0.2925208f)))
				.WithRange(2, new RangeSquare(this, new Vector2(0.49047527f, 0.29142094f)))
				.WithPierce(2, new PierceSquare(this, new Vector2(0.7194155f, 0.29152092f)))
				.WithPull(1)
				.Build())
		];

		public override int XP => 1;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(InfuseElementIfLightAbility(Element.Light)),
			new AbilityCardAbility(CreateThornsAbilityBuilder()
				.WithRange(0)
				.Build()),
			new AbilityCardAbility(HealAbility.Builder()
				.WithHealValue(3, new HealSquare(this, new Vector2(0.49435562f, 0.86845195f)))
				.WithTarget(Target.Self)
				.Build())
		];
	}
}