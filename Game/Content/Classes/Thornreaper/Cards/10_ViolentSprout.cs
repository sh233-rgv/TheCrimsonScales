using System;
using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class ViolentSprout : ThornreaperCardModel<ViolentSprout.CardTop, ViolentSprout.CardBottom>
{
	public override string Name => "Violent Sprout";
	public override int Level => 1;
	public override int Initiative => 63;
	protected override int AtlasIndex => 10;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(CreateThornsAbilityBuilder()
				.WithRange(3)
				.Build()),
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2, new AttackSquare(this, new Vector2(0.30887526f, 0.37506926f)))
				.WithTargets(3)
				.WithCustomGetTargets((state, figures) =>
				{
					figures.AddRange(RangeHelper.GetFiguresInRange(
						state.ActionState.GetAbilityState<CreateOverlayTileAbility<ThornsThornreaper>.State>(0).CreatedOverlayTiles[0], 1));
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
			new AbilityCardAbility(InfuseElementIfLightAbility(Element.Earth)),
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(1, new AttackSquare(this, new Vector2(0.27307612f, 0.80866426f)))
				.WithTarget(Target.Enemies | Target.TargetAll)
				.WithConditions(Conditions.Immobilize)
				.Build())
		];
	}
}