using System;
using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class WelcomeToTheJungle : ThornreaperCardModel<WelcomeToTheJungle.CardTop, WelcomeToTheJungle.CardBottom>
{
	public override string Name => "Welcome to the Jungle";
	public override int Level => 3;
	public override int Initiative => 41;
	protected override int AtlasIndex => 17;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(3, new AttackSquare(this, new Vector2(0.49668384f, 0.17396124f)))
				.WithConditions(Conditions.Muddle)
				.WithAOEPattern(new AOEPattern(
						[
							new AOEHex(Vector2I.Zero, AOEHexType.Gray),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthWest).Add(Direction.NorthEast), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthEast).Add(Direction.East), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthEast).Add(Direction.East).Add(Direction.NorthEast), AOEHexType.Red),
						]
					), new AOEHexMark(Vector2I.Zero.Add(Direction.SouthEast), this, new Vector2(0.45011973f, 0.4470914f)),
					new AOEHexMark(Vector2I.Zero.Add(Direction.East), this, new Vector2(0.50056416f, 0.3850416f)),
					new AOEHexMark(Vector2I.Zero.Add(Direction.East).Add(Direction.East), this, new Vector2(0.5999009f, 0.3850416f)))
				.Build())
		];

		public override Func<Figure, GDTask<bool>> OnCardSideStarted => ActionConsumeEarth;

		public override int XP => 1;
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(PullAbility.Builder()
				.WithPull(2)
				.WithRange(3, new RangeSquare(this, new Vector2(0.623959f, 0.7069252f)))
				.Build()),
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					await AbilityCmd.AddCondition(state, state.Performer, Conditions.Strengthen);
					await AbilityCmd.GainXP(state.Performer, 1);
				})
				.WithConditionalAbilityCheck(async state =>
				{
					PullAbility.State pullState = state.ActionState.GetAbilityState<PullAbility.State>(0);

					await GDTask.CompletedTask;
					return pullState.SingleTargetStates.Any(singleTargetState =>
						singleTargetState.PullHexes.Any(hex => hex.HasHexObjectOfType<HazardousTerrain>()));
				})
				.Build())
		];
	}
}