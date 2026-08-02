using System;
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class ImpalingCommand : ThornreaperCardModel<ImpalingCommand.CardTop, ImpalingCommand.CardBottom>
{
	public override string Name => "Impaling Command";
	public override int Level => 8;
	public override int Initiative => 19;
	protected override int AtlasIndex => 27;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(InfuseElementIfLightAbility(Element.Light)),
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(6, new AttackSquare(this, new Vector2(0.27550432f, 0.33196402f)))
				.WithPierce(3)
				.WithCustomGetTargets((state, figures) =>
				{
					figures.AddRange(RangeHelper.GetFiguresInRange(state.Performer, 5)
						.Where(figure => figure.Hex.HasHexObjectOfType<HazardousTerrain>()));
				})
				.Build())
		];
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3, new AttackSquare(this, new Vector2(0.46098468f, 0.67686224f)))
				.WithAOEPattern(new AOEPattern(
						[
							new AOEHex(Vector2I.Zero, AOEHexType.Gray),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthEast).Add(Direction.NorthEast), AOEHexType.Red),
						]
					),
					new AOEHexMark(Vector2I.Zero.Add(Direction.East).Add(Direction.NorthEast), this, new Vector2(0.7351128f, 0.6759003f)))
				.Build()),
			new AbilityCardAbility(CreateThornsAbilityBuilder()
				.WithCustomSelectHexes((state, hexes) =>
				{
					hexes.AddRange(state.ActionState.GetAbilityState<AttackAbility.State>(0).GetRedAOEHexes().Where(hex => hex.IsFeatureless()));
				})
				.WithConditionalAbilityCheck(state => AbilityCmd.HasPerformedAbility(state, 0))
				.Build())
		];
	}
}