using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class HeatWaves : LuminaryCardModel<HeatWaves.CardTop, HeatWaves.CardBottom>
{
	public override string Name => "Heat Waves";
	public override int Level => 1;
	public override int Initiative => 73;
	protected override int AtlasIndex => 4;

	public class CardTop : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			Glow([Element.Fire], GlowAbility)
		];

		protected override IEnumerable<Element> Elements => [Element.Light];
		protected override int XP => 1;
		protected override bool Persistent => true;

		protected Ability GlowAbility(List<Element> elements)
        {
            return ConditionAbility.Builder()
				.WithConditions(Conditions.Wound1)
				.WithAOEPattern(new AOEPattern(
					[
						new AOEHex(Vector2I.Zero, AOEHexType.Gray),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthWest), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
						new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
					]
				))
				.WithOnAbilityStarted(async state =>
				{
					state.SetCustomValue(state.Performer, "Glow Ability", true);
					state.SetCustomValue(state.Performer, "Consumed Elements", new List<Element>{Element.Fire});

					await GDTask.CompletedTask;
				})
				.Build();
        }
	}

	public class CardBottom : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(2)
				.Build())
		];

		protected override IEnumerable<Element> Elements => [Element.Ice];
	}
}