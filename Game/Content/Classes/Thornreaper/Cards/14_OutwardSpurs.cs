using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class OutwardSpurs : ThornreaperCardModel<OutwardSpurs.CardTop, OutwardSpurs.CardBottom>
{
	public override string Name => "Outward Spurs";
	public override int Level => 2;
	public override int Initiative => 43;
	protected override int AtlasIndex => 14;

	public class CardTop : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(CreateThornsAbilityBuilder()
				.WithRange(0)
				.Build()),
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2, new AttackSquare(this, new Vector2(0.49648386f, 0.23434904f)))
				.WithPierce(2)
				.WithAOEPattern(new AOEPattern(
						[
							new AOEHex(Vector2I.Zero, AOEHexType.Gray),
							new AOEHex(Vector2I.Zero.Add(Direction.West), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthEast).Add(Direction.East), AOEHexType.Red),
							new AOEHex(Vector2I.Zero.Add(Direction.SouthEast), AOEHexType.Red),
						]
					),
					new AOEHexMark(Vector2I.Zero.Add(Direction.West).Add(Direction.NorthWest), this, new Vector2(0.32594875f, 0.30914947f)),
					new AOEHexMark(Vector2I.Zero.Add(Direction.West).Add(Direction.SouthWest), this, new Vector2(0.32517272f, 0.432687f)))
				.Build())
		];
	}

	public class CardBottom : ThornreaperCardSide
	{
		protected override List<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(SufferDamageAbility.Builder()
				.WithDamage(1)
				.WithRange(1)
				.WithTarget(Target.TargetAll | Target.Enemies)
				.Build()),
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(2, new MoveSquare(this, new Vector2(0.6696471f, 0.7556787f)))
				.WithOnAbilityStarted(async state =>
				{
					SufferDamageAbility.State sufferDamageState = state.ActionState.GetAbilityState<SufferDamageAbility.State>(0);
					state.AdjustMoveValue(sufferDamageState.UniqueTargetedFigures.Count);

					await GDTask.CompletedTask;
				})
				.Build())
		];
	}
}