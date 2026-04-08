using Fractural.Tasks;
using Godot;

public class AllStartScenarioWithDamageReward(int damage) : Reward
{
	public override RewardType Type => RewardType.ScenarioStart;

	public override string GetLabelText(RichTextParameters textParameters) =>
		$"All characters start the next scenario with {Icons.Inline(Icons.Damage, textParameters)}{damage}.";

	public override async GDTask OnScenarioSetupPhaseCompleted()
	{
		await base.OnScenarioSetupPhaseCompleted();

		foreach(Character character in GameController.Instance.CharacterManager.Characters)
		{
			ScenarioEvents.SufferDamageEventReward.Parameters sufferDamageParameters =
				await ScenarioEvents.SufferDamageEventRewardEvent.CreatePrompt(
					new ScenarioEvents.SufferDamageEventReward.Parameters(character), character);

			if(!sufferDamageParameters.Prevented)
			{
				await AbilityCmd.SufferDamage(character, damage, character);
			}
		}
	}
}