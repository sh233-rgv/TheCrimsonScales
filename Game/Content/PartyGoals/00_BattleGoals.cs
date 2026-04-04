using Fractural.Tasks;

public class BattleGoalsPartyGoal : ScalesWithCharactersPartyGoalModel
{
	public override int MaxProgress => 5;

	public override string GetText(int characterCount) => $"Complete 5 battle goals with {characterCount} characters";

	protected override async GDTask OnScenarioSetupPhaseCompleted(ScalesWithCharactersPartyGoalData partyGoalData)
	{
		await base.OnScenarioSetupPhaseCompleted(partyGoalData);

		GameController.Instance.BattleGoalCompletedEvent += (character, model) =>
		{
			partyGoalData.AdjustProgress(1, character);
		};
	}
}