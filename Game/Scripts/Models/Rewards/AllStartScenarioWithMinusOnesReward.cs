using Fractural.Tasks;
using Godot;

public class AllStartScenarioWithMinusOnesReward(int number) : Reward
{
	public override RewardType Type => RewardType.ScenarioStart;

	public override string GetLabelText(RichTextParameters parameters) =>
		$"All characters start the next scenario with {Icons.Inline(Icons.MinusOneCard)} x{number}.";

	public override async GDTask OnScenarioSetupPhaseCompleted()
	{
		await base.OnScenarioSetupPhaseCompleted();

		foreach(Character character in GameController.Instance.CharacterManager.Characters)
		{
			ScenarioEvents.AddMinusOnesEventReward.Parameters inflictConditionsParameters =
				await ScenarioEvents.AddMinusOnesEventRewardEvent.CreatePrompt(
					new ScenarioEvents.AddMinusOnesEventReward.Parameters(character), character);

			if(!inflictConditionsParameters.Prevented)
			{
				for(int i = 0; i < number; i++)
				{
					character.AMDCardDeck.AddMinusOne();
				}
			}
		}
	}
}