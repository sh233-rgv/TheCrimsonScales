using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class ShinyDiversion : LuminaryCardModel<ShinyDiversion.CardTop, ShinyDiversion.CardBottom>
{
	public override string Name => "Shiny Diversion";
	public override int Level => 3;
	public override int Initiative => 29;
	protected override int AtlasIndex => 17;

	public class CardTop : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			Glow(Element.Light, OtherAbility.Builder()
				.WithPerformAbility(async state =>
                {
                    ActionState grantAction = new ActionState(state.Performer, [GrantAbility.Builder()
						.WithGetAbilities(state =>
						[
							ShieldAbility.Builder()
								.WithShieldValue(1)
								.WithOnAbilityEndedPerformed(async state =>
                                {
                                    await AbilityCmd.InfuseElement(Element.Ice, state.Authority, state);
                                })
								.Build(),
						])
						.WithAOEPattern(new AOEPattern(
							[
								new AOEHex(Vector2I.Zero, AOEHexType.Gray),
								new AOEHex(Vector2I.Zero.Add(Direction.West), AOEHexType.Red),
								new AOEHex(Vector2I.Zero.Add(Direction.NorthWest), AOEHexType.Red | AOEHexType.Marked),
								new AOEHex(Vector2I.Zero.Add(Direction.NorthWest).Add(Direction.NorthEast), AOEHexType.Red | AOEHexType.Marked),
								new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red | AOEHexType.Marked),
								new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Red),
							]
						)).Build()]);
					await grantAction.Perform();
					foreach(Hex hex in grantAction.GetAbilityState<GrantAbility.State>(0).GetMarkedAOEHexes())
					{
						await AbilityCmd.LootHex(state.Performer, hex);
					}
                })
				.WithOnAbilityStarted(async state =>
				{
					state.SetCustomValue("Glow", "Glow Ability", true);

					await GDTask.CompletedTask;
				})
				.Build())
		];
		protected override int XP => 1;
		protected override bool Persistent => true;
	}

	public class CardBottom : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(3)
				.Build()),
			new AbilityCardAbility(ConditionAbility.Builder()
				.WithConditions(Conditions.Muddle)
				.WithTarget(Target.Enemies | Target.TargetAll)
				.WithRange(1)
				.Build()),
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(2)
				.Build()),
		];
	}
}