using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;

public class ContagiousMelody : BrightsparkCardModel<ContagiousMelody.CardTop, ContagiousMelody.CardBottom>
{
	public override string Name => "Contagious Melody";
	public override int Level => 1;
	public override int Initiative => 13;
	protected override int AtlasIndex => 3;

	public class CardTop : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2)
				.WithRange(2)
				.WithConditions(Conditions.Poison1)
				.Build()),
			new AbilityCardAbility(ConditionAbility.Builder()
				.WithConditions(Conditions.Poison1)
				.WithCustomGetTargets((state, targets) =>
				{
					AttackAbility.State attackAbilityState = state.ActionState.GetAbilityState<AttackAbility.State>(0);
					targets.AddRange(
						attackAbilityState.UniqueTargetedFigures.SelectMany(figure =>
							RangeHelper.GetFiguresInRange(figure.Hex, 1, false)));
				})
				.WithConditionalAbilityCheck(state => AbilityCmd.HasPerformedAbility(state, 0))
				.WithOnAbilityEndedPerformed(async state =>
				{
					await AbilityCmd.GainXP(state.Performer, 1);
				})
				.Build())
		];
	}

	public class CardBottom : BrightsparkCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(MoveAbility.Builder()
				.WithDistance(2)
				.Build()),
			new AbilityCardAbility(OtherAbility.Builder()
				.WithPerformAbility(async state =>
				{
					ConditionModel conditionGiven = null;
					List<ScenarioEvents.GenericChoice.Subscription> subscriptions = [];
					foreach(ConditionModel conditionModel in state.Performer.Conditions.Where(conditionModel => conditionModel.IsNegative))
					{
						subscriptions.Add(ScenarioEvents.GenericChoice.Subscription.New(
							applyFunction: async applyParameters =>
							{
								conditionGiven = conditionModel;
								await GDTask.CompletedTask;
							},
							effectType: EffectType.Selectable,
							effectButtonParameters: new IconEffectButton.Parameters(Icons.GetCondition(conditionModel)),
							effectInfoViewParameters: new TextEffectInfoView.Parameters($"Give {Icons.Inline(Icons.GetCondition(conditionModel))}")
						));
					}

					await AbilityCmd.GenericChoice(state.Performer, subscriptions, hintText: "Select a condition to remove");
					if(conditionGiven == null)
					{
						return;
					}

					Figure figure = await AbilityCmd.SelectFigure(state,
						figures => figures.AddRange(RangeHelper.GetFiguresInRange(state.Performer.Hex, 1)
							.Where(figure => figure.EnemiesWith(state.Performer))),
						hintText: () => $"Select an enemy to give {Icons.Inline(Icons.GetCondition(conditionGiven))}");
					if(figure == null)
					{
						return;
					}

					await AbilityCmd.AddCondition(state, figure, conditionGiven);
					state.SetPerformed();
					await AbilityCmd.GainXP(state.Performer, 1);
				})
				.Build())
		];
	}
}