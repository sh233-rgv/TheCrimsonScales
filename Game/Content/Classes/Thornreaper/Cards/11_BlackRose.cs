using System;
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class BlackRose : ThornreaperCardModel<BlackRose.CardTop, BlackRose.CardBottom>
{
	public override string Name => "Black Rose";
	public override int Level => 1;
	public override int Initiative => 12;
	protected override int AtlasIndex => 11;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2, new AttackDiamond(this, new Vector2(0.34983075f, 0.25152355f)))
				.WithTarget(Target.Enemies | Target.TargetAll)
				.WithRange(2)
				.WithRangeType(RangeType.Melee)
				.WithConditions(Conditions.Immobilize)
				.Build())
		];

		public override IEnumerable<CardElementInfusion> Elements => [CardElementInfusion.Infuse(Element.Light)];
		public override int XP => 2;
		public override bool Loss => true;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(CreateThornsAbilityBuilder()
				.WithCount(int.MaxValue)
				.WithCustomSelectHexes((state, hexes) =>
				{
					hexes.AddRange(RangeHelper.GetHexesInRange(state.Performer.Hex, 1).Where(hex => hex.IsFeatureless() &&
						hex.GetFigures().Any(figure => figure == state.Performer || figure.EnemiesWith(state.Performer))));
				})
				.Build()),
			new AbilityCardAbility(SufferDamageAbility.Builder()
				.WithDamage(1)
				.WithTarget(Target.Enemies | Target.TargetAll)
				.WithRange(1)
				.Build()),
			new AbilityCardAbility(PushAbility.Builder()
				.WithPush(2, new PushDiamond(this, new Vector2(0.45099577f, 0.82296866f)))
				.WithTarget(Target.Enemies | Target.TargetAll)
				.WithRange(1)
				.Build())
		];

		public override IEnumerable<CardElementInfusion> Elements => [CardElementInfusion.Infuse(Element.Dark)];
		public override int XP => 2;
		public override bool Loss => true;
	}
}