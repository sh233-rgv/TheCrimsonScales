using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class Moonbeam : LuminaryCardModel<Moonbeam.CardTop, Moonbeam.CardBottom>
{
	public override string Name => "Moonbeam";
	public override int Level => 1;
	public override int Initiative => 85;
	protected override int AtlasIndex => 5;

	public class CardTop : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			Glow(Element.Dark, ConditionAbility.Builder()
				.WithConditions(Conditions.Curse)
				.WithAOEPattern(new AOEPattern(
					[
						new AOEHex(Vector2I.Zero, AOEHexType.Gray),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthWest), AOEHexType.Red),
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

		protected override IEnumerable<Element> Elements => [Element.Ice];
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

		protected override IEnumerable<Element> Elements => [Element.Fire];
	}
}