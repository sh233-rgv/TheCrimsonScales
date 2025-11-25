using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class RadiantGlare : LuminaryCardModel<RadiantGlare.CardTop, RadiantGlare.CardBottom>
{
	public override string Name => "Radiant Glare";
	public override int Level => 1;
	public override int Initiative => 36;
	protected override int AtlasIndex => 6;

	public class CardTop : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			Glow(Element.Light, ConditionAbility.Builder()
				.WithConditions(Conditions.Immobilize)
				.WithAOEPattern(new AOEPattern(
					[
						new AOEHex(Vector2I.Zero, AOEHexType.Gray),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthEast).Add(Direction.East), AOEHexType.Red),
					]
				))
				.WithOnAbilityStarted(async state =>
				{
					state.SetCustomValue("Glow", "Glow Ability", true);

					await GDTask.CompletedTask;
				})
				.Build())
		];

		protected override IEnumerable<Element> Elements => [Element.Fire];
		protected override int XP => 1;
		protected override bool Persistent => true;
	}

	public class CardBottom : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(2)
				.Build())
		];

		protected override IEnumerable<Element> Elements => [Element.Dark];
	}
}