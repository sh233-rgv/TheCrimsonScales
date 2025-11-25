using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class SoftGlow : LuminaryCardModel<SoftGlow.CardTop, SoftGlow.CardBottom>
{
	public override string Name => "Soft Glow";
	public override int Level => 1;
	public override int Initiative => 24;
	protected override int AtlasIndex => 8;

	public class CardTop : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			Glow(Element.Ice, ConditionAbility.Builder()
				.WithConditions(Conditions.Strengthen)
				.WithAOEPattern(new AOEPattern(
					[
						new AOEHex(Vector2I.Zero, AOEHexType.Gray),
						new AOEHex(Vector2I.Zero.Add(Direction.West), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthWest), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
					]
				))
				.WithOnAbilityStarted(async state =>
				{
					state.SetCustomValue("Glow", "Glow Ability", true);

					await GDTask.CompletedTask;
				})
				.Build())
		];

		protected override IEnumerable<Element> Elements => [Element.Dark];
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

		protected override IEnumerable<Element> Elements => [Element.Light];
	}
}