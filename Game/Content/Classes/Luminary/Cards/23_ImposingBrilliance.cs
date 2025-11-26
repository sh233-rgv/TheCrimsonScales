using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class ImposingBrilliance : LuminaryCardModel<ImposingBrilliance.CardTop, ImposingBrilliance.CardBottom>
{
	public override string Name => "Imposing Brilliance";
	public override int Level => 6;
	public override int Initiative => 86;
	protected override int AtlasIndex => 23;

	public class CardTop : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3)
				.WithAOEPattern(new AOEPattern(
					[
						new AOEHex(Vector2I.Zero, AOEHexType.Gray),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red | AOEHexType.Marked),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthWest), AOEHexType.Red | AOEHexType.Marked),
						new AOEHex(Vector2I.Zero.Add(Direction.NorthWest).Add(Direction.NorthEast), AOEHexType.Red | AOEHexType.Marked2),
						new AOEHex(Vector2I.Zero.Add(Direction.East), AOEHexType.Empty),
						new AOEHex(Vector2I.Zero.Add(Direction.West), AOEHexType.Empty),
					]
				))
				.WithAfterTargetConfirmedSubscriptions(
				[
					ScenarioEvents.AttackAfterTargetConfirmed.Subscription.New(
						parameters => parameters.AbilityState.GetMarkedAOEHexes().Contains(parameters.AbilityState.Target.Hex),
						async parameters =>
						{
							parameters.AbilityState.SingleTargetAddCondition(Conditions.Poison1);

							await GDTask.CompletedTask;
						}
					),
					ScenarioEvents.AttackAfterTargetConfirmed.Subscription.New(
						parameters => parameters.AbilityState.GetMarked2AOEHexes().Contains(parameters.AbilityState.Target.Hex),
						async parameters =>
						{
							parameters.AbilityState.SingleTargetAddCondition(Conditions.Immobilize);

							await GDTask.CompletedTask;
						}
					)
				])
				.Build()),
			Scuttle(1, [Element.Dark]),
		];
	}

	public class CardBottom : LuminaryCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(2)
				.WithOnAbilityEndedPerformed(async state =>
                {
					if (RangeHelper.GetFiguresInRange(state.Performer.Hex, 1, false).Any(figure => figure.EnemiesWith(state.Performer)))
                    {
                        await AbilityCmd.InfuseElement(Element.Fire, state.Authority, state);
                    }
					if (RangeHelper.GetFiguresInRange(state.Performer.Hex, 1, false).Any(figure => figure.AlliedWith(state.Performer)))
                    {
                        await AbilityCmd.InfuseElement(Element.Ice, state.Authority, state);
                    }
					if (state.Origin == state.Performer.Hex)
                    {
                        await AbilityCmd.InfuseElement(Element.Light, state.Authority, state);
                    }
                    await GDTask.CompletedTask;
                })
				.Build()),
		];
	}
}