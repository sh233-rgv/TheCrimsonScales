using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class Sungaze : StarslingerCardModel<Sungaze.CardTop, Sungaze.CardBottom>
{
	public override string Name => "Sungaze";
	public override int Level => 8;
	public override int Initiative => 37;
	protected override int AtlasIndex => 26;

	public class CardTop : StarslingerCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(HealAbility.Builder()
				.WithHealValue(4)
				.WithTarget(Target.Allies | Target.TargetAll)
				.WithRange(1)
				.WithConditions(Conditions.Strengthen)
				.Build()),
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
					{
						HealAbility.State healAbilityState = state.ActionState.GetAbilityState<HealAbility.State>(0);
						if (healAbilityState.UniqueTargetedFigures.Count == 1)
						{
							await AbilityCmd.AddCondition(state, healAbilityState.UniqueTargetedFigures[0], Conditions.Bless);
						}
						await GDTask.CompletedTask;
					}
				)
				.Build())
		];

		protected override IEnumerable<Element> Elements => [Element.Dark];
	}

	public class CardBottom : StarslingerCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(4)
				.WithAOEPattern(new AOEPattern([
							new AOEHex(Vector2I.Zero, AOEHexType.Gray),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthEast), AOEHexType.Yellow),
							new AOEHex(Vector2I.Zero.Add(Direction.NorthEast).Add(Direction.NorthEast), AOEHexType.Red),
						]))
				.Build()),
			new AbilityCardAbility(ConditionAbility.Builder()
				.WithConditions(Conditions.Strengthen)
				.WithCustomGetTargets((abilityState, list) =>
				{
					AttackAbility.State attackAbilityState = abilityState.ActionState.GetAbilityState<AttackAbility.State>(0);

					if(attackAbilityState.Performed)
					{
						foreach(Hex yellowHex in attackAbilityState.GetYellowAOEHexes())
						{
							foreach(Figure figure in yellowHex.GetHexObjectsOfType<Figure>())
							{
								list.Add(figure);
							}
						}
					}
				})
				.WithOnAbilityEnded(async state =>
					{
						if (state.Performed)
						{
							await AbilityCmd.GainXP(state.Performer, 1);
						}
					}
				)
				.Build())
		];
	}
}