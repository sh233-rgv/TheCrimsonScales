using System.Collections.Generic;
using Fractural.Tasks;

public class KindledTonic : FireKnightItem
{
	public override string Name => "Kindled Tonic";
	public override int ItemNumber => 6;
	protected override int AtlasIndex => 10 - 6;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringTurn(
			canApply: character => character == Owner,
			apply: async character =>
			{
				await Use(async user =>
				{
					List<ScenarioEvents.GenericChoice.Subscription> subscriptions = new List<ScenarioEvent<ScenarioEvents.GenericChoice.Parameters>.Subscription>();
					foreach(ConditionModel conditionModel in user.Conditions)
					{
						if(conditionModel.IsNegative)
						{
							subscriptions.Add(ScenarioEvents.GenericChoice.Subscription.New(
								applyFunction: async applyParameters =>
								{
									await AbilityCmd.RemoveCondition(user, conditionModel);
								},
								effectType: EffectType.SelectableMandatory,
								effectButtonParameters: new IconEffectButton.Parameters(Icons.GetCondition(conditionModel)),
								effectInfoViewParameters: new TextEffectInfoView.Parameters($"Remove {Icons.Inline(Icons.GetCondition(conditionModel))}")
							));
						}
					}

					await AbilityCmd.GenericChoice(user, subscriptions, hintText: "Select a condition to remove");

					object subscriber = new object();
					ScenarioEvents.FigureTurnEndingEvent.Subscribe(user, subscriber,
						parameters => parameters.Figure == user,
						async parameters =>
						{
							ScenarioEvents.FigureTurnEndingEvent.Unsubscribe(user, subscriber);

							await AbilityCmd.AddCondition(null, user, Conditions.Strengthen);

							if(await AbilityCmd.AskConsumeElement(user, Element.Fire, effectInfoText: $"{Icons.Inline(Icons.GetCondition(Conditions.Bless))}"))
							{
								await AbilityCmd.AddCondition(null, user, Conditions.Bless);
							}
						}
					);
				});
			}
		);
	}
}