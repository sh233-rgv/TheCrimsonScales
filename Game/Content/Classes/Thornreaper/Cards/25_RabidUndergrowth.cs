using System;
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class RabidUndergrowth : ThornreaperCardModel<RabidUndergrowth.CardTop, RabidUndergrowth.CardBottom>
{
	public override string Name => "Rabid Undergrowth";
	public override int Level => 7;
	public override int Initiative => 27;
	protected override int AtlasIndex => 25;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3, new AttackSquare(this, new Vector2(0.2786086f, 0.2465374f)))
				.WithRange(3)
				.WithPierce(2)
				.WithAOEPattern(new AOEPattern(
				[
					new AOEHex(Vector2I.Zero, AOEHexType.Red),
					new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
					new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
					new AOEHex(Vector2I.Zero.Add(Direction.East).Add(Direction.East), AOEHexType.Red),
					new AOEHex(Vector2I.Zero.Add(Direction.East).Add(Direction.East).Add(Direction.NorthEast), AOEHexType.Red),
				]))
				.Build()),
			new AbilityCardAbility(CreateThornsAbilityBuilder()
				.WithCount(int.MaxValue)
				.WithCustomSelectHexes((state, hexes) =>
				{
					hexes.AddRange(state.ActionState.GetAbilityState<AttackAbility.State>(0).GetRedAOEHexes().Where(hex => hex.IsFeatureless()));
				})
				.WithConditionalAbilityCheck(state => AbilityCmd.HasPerformedAbility(state, 0))
				.Build())
		];

		public override Func<Figure, GDTask<bool>> OnCardSideStarted => ActionConsumeEarth;

		public override int XP => 2;
		public override bool Loss => true;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(InfuseElementIfLightAbility(Element.Earth)),
			new AbilityCardAbility(CreateThornsAbilityBuilder()
				.WithRange(1)
				.Build()),
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3, new AttackSquare(this, new Vector2(0.32672483f, 0.86149585f)))
				.WithCustomGetPerformHex(state =>
					state.ActionState.GetAbilityState<CreateOverlayTileAbility<ThornsThornreaper>.State>(1).CreatedOverlayTiles.First().Hex)
				.WithConditionalAbilityCheck(state => AbilityCmd.HasPerformedAbility(state, 1))
				.Build())
		];
	}
}