using Fractural.Tasks;
using Godot;

public class AllStartScenarioWithConditionReward(params ConditionModel[] conditionModels) : Reward
{
	public override RewardType Type => RewardType.ScenarioStart;

	public override string GetLabelText(RichTextParameters parameters)
	{
		string labelText = "All characters start the next scenario with ";

		for(int i = 0; i < conditionModels.Length; i++)
		{
			ConditionModel conditionModel = conditionModels[i];
			if(i > 0)
			{
				labelText += ", ";
			}

			labelText += Icons.Inline(Icons.GetCondition(conditionModel));
		}

		labelText += ".";

		return labelText;
	}

	public override async GDTask OnScenarioSetupPhaseCompleted()
	{
		await base.OnScenarioSetupPhaseCompleted();

		foreach(Character character in GameController.Instance.CharacterManager.Characters)
		{
			foreach(ConditionModel conditionModel in conditionModels)
			{
				ScenarioEvents.InflictConditionEventReward.Parameters inflictConditionsParameters =
					await ScenarioEvents.InflictConditionEventRewardEvent.CreatePrompt(
						new ScenarioEvents.InflictConditionEventReward.Parameters(character, conditionModel), character);

				if(!inflictConditionsParameters.Prevented)
				{
					await AbilityCmd.AddCondition(null, character, conditionModel);
				}
			}
		}
	}
}