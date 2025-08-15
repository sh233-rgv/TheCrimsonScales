using System.Collections.Generic;
using Fractural.Tasks;

public class Safeguard : ConditionModel
{
	public override string Name => "Safeguard";
	public override string IconPath => "res://Art/Icons/ConditionsAndEffects/Safeguard.svg";
	public override bool IsPositive => true;

	public override async GDTask Add(Figure target, ConditionNode node)
	{
		await base.Add(target, node);

		ScenarioEvents.InflictConditionsEvent.Subscribe(this,
			parameters => parameters.Target == Owner && parameters.ConditionModels.Count > 0,
			async parameters =>
			{
				Node.Flash();

				List<ScenarioEvents.GenericChoice.Subscription> subscriptions = new List<ScenarioEvent<ScenarioEvents.GenericChoice.Parameters>.Subscription>();
				foreach(ConditionModel conditionModel in parameters.ConditionModels)
				{
					subscriptions.Add(ScenarioEvents.GenericChoice.Subscription.New(
						applyFunction: async applyParameters =>
						{
							parameters.PreventCondition(conditionModel);

							await GDTask.CompletedTask;
						},
						effectType: EffectType.SelectableMandatory,
						effectButtonParameters: new IconEffectButton.Parameters(Icons.GetCondition(conditionModel)),
						effectInfoViewParameters: new TextEffectInfoView.Parameters($"Prevent {Icons.Inline(Icons.GetCondition(conditionModel))}")
					));
				}

				await AbilityCmd.GenericChoice(Owner, subscriptions, hintText: "Select a condition to prevent");

				await AbilityCmd.RemoveCondition(target, this);
			},
			EffectType.MandatoryBeforeOptionals, 100
		);
	}

	public override async GDTask Remove()
	{
		await base.Remove();

		ScenarioEvents.InflictConditionsEvent.Unsubscribe(this);
	}
}