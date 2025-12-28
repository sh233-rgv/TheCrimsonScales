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
			Glow(new GlowAbilityModel([Element.Ice], GlowAbility,
				$"Perform {Icons.Inline(Icons.GetCondition(Conditions.Strengthen))} ability", Icons.GetCondition(Conditions.Strengthen)))
		];

		protected override IEnumerable<Element> Elements => [Element.Dark];
		protected override int XP => 1;
		protected override bool Persistent => true;

		private Ability GlowAbility(List<Element> elements)
        {
            return ConditionAbility.Builder()
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
					state.SetCustomValue(state.Performer, "Glow Ability", true);
					state.SetCustomValue(state.Performer, "Consumed Elements", elements);

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

		protected override IEnumerable<Element> Elements => [Element.Light];
	}
}