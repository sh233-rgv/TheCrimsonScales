using System.Collections.Generic;
using Fractural.Tasks;

public class PersonalPoison : MirefootCardModel<PersonalPoison.CardTop, PersonalPoison.CardBottom>
{
	public override string Name => "Personal Poison";
	public override int Level => 5;
	public override int Initiative => 86;
	protected override int AtlasIndex => 19;

	public class CardTop : MirefootCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					MonsterGroup monsterGroup = null;
					List<ScenarioEvents.GenericChoice.Subscription> subscriptions = [];
					foreach(MonsterGroup group in GameController.Instance.Scenario.Map.MonsterGroups)
					{
						subscriptions.Add(ScenarioEvent<ScenarioEvents.GenericChoice.Parameters>.Subscription.New(
							subscriptionParameters => true,
							async subscriptionParameters =>
							{
								monsterGroup = group;

								await GDTask.CompletedTask;
							},
							effectType: EffectType.SelectableMandatory,
							effectButtonParameters: new IconEffectButton.Parameters(group.MonsterModel.PortraitTexturePath),
							effectInfoViewParameters: new TextEffectInfoView.Parameters(
								$"Place a character token on {group.MonsterModel.Name}s")));
					}

					await AbilityCmd.GenericChoice(state.Performer,
						subscriptions, hintText: "Choose a group of monsters to place a character token on");

					if(monsterGroup == null)
					{
						return;
					}

					ScenarioEvents.InflictConditionEvent.Subscribe(state, this,
						parameters => parameters.Target is Monster monster && monsterGroup.Monsters.Contains(monster) &&
						              parameters.ConditionModel is Poison,
						async parameters =>
						{
							await AbilityCmd.AddCondition(state, parameters.Target, Conditions.Wound1);
						});
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.InflictConditionEvent.Unsubscribe(state, this);
					await GDTask.CompletedTask;
				})
				.Build()),
			new AbilityCardAbility(AttackAbility.Builder()
				.WithDamage(2)
				.WithConditions(Conditions.Poison2)
				.Build())
		];

		protected override int XP => 1;
		protected override bool Persistent => true;
	}

	public class CardBottom : MirefootCardSide
	{
		protected override IEnumerable<AbilityCardAbility> GetAbilities() =>
		[
			new AbilityCardAbility(ConditionAbility.Builder()
				.WithConditions(Conditions.Wound1, Conditions.Poison1)
				.WithTarget(Target.Self)
				.WithMandatory(true)
				.Build()),
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.DuringAttackEvent.Subscribe(state, this,
						parameters => parameters.Performer == state.Performer && state.Performer.HasCondition(Conditions.Poison1) &&
						              state.Performer.HasCondition(Conditions.Wound1),
						async parameters =>
						{
							parameters.AbilityState.SingleTargetAdjustAttackValue(1);
							parameters.AbilityState.SingleTargetAdjustPierce(2);
							await GDTask.CompletedTask;
						});
					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					ScenarioEvents.DuringAttackEvent.Unsubscribe(state, this);
					await GDTask.CompletedTask;
				})
				.Build()),
			new AbilityCardAbility(OtherActiveAbility.Builder()
				.WithOnActivate(async state =>
				{
					ScenarioEvents.AbilityCardStateChangedEvent.Subscribe(state, this,
						parameters => parameters.AbilityCard == AbilityCard && parameters.AbilityCard.CardState == CardState.Discarded,
						async parameters =>
						{
							await new ActionState(state.Performer, [HealAbility.Builder().WithHealValue(1).WithTarget(Target.Self).Build()])
								.Perform();
							ScenarioEvents.AbilityCardStateChangedEvent.Unsubscribe(state, this);
						});
					await GDTask.CompletedTask;
				})
				.WithOnDeactivate(async state =>
				{
					await GDTask.CompletedTask;
				})
				.Build())
		];

		protected override bool Persistent => true;
	}
}