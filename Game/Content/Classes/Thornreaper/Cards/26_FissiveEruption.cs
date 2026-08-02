using System;
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class FissiveEruption : ThornreaperCardModel<FissiveEruption.CardTop, FissiveEruption.CardBottom>
{
	public override string Name => "Fissive Eruption";
	public override int Level => 8;
	public override int Initiative => 40;
	protected override int AtlasIndex => 26;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(CreateThornsAbilityBuilder()
				.WithRange(4)
				.WithCount(2)
				.Build()),
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(4, new AttackSquare(this, new Vector2(0.3104274f, 0.37396124f)))
				.WithTargets(2)
				.WithCustomGetTargets((state, figures) =>
				{
					figures.AddRange(state.ActionState.GetAbilityState<CreateOverlayTileAbility<ThornsThornreaper>.State>(0).CreatedOverlayTiles
						.SelectMany(thorns => thorns.Hex.GetFigures()));
				})
				.WithConditionalAbilityCheck(state => AbilityCmd.HasPerformedAbility(state, 0))
				.Build())
		];

		public override Func<Figure, GDTask<bool>> OnCardSideStarted => ActionConsumeEarth;

		public override int XP => 1;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(4, new MoveSquare(this, new Vector2(0.5227701f, 0.6609419f)))
				.WithMoveType(MoveType.Jump)
				.Build()),
			new AbilityCardAbility(CreateThornsAbilityBuilder()
				.WithRange(0)
				.Build()),
		];

		public override IEnumerable<CardElementInfusion> Elements => [CardElementInfusion.Infuse(Element.Light)];
	}
}