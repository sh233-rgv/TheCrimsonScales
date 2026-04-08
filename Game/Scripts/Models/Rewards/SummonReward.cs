using Fractural.Tasks;
using Godot;

public class SummonReward(SummonAbility summonAbility) : Reward
{
	public override RewardType Type => RewardType.ScenarioStart;

	public override string GetLabelText(RichTextParameters textParameters) =>
		$"At the start of the next scenario, one character may Summon a {summonAbility.Name}.";

	public override async GDTask OnScenarioSetupPhaseCompleted()
	{
		await base.OnScenarioSetupPhaseCompleted();

		Figure selectedFigure = await AbilityCmd.SelectFigure(GameController.Instance.CharacterManager.GetCharacter(0),
			list =>
			{
				list.AddRange(GameController.Instance.CharacterManager.Characters);
			}, hintText: () => $"Select a character to Summon a {summonAbility.Name}"
		);

		if(selectedFigure != null)
		{
			ActionState actionState = new ActionState(selectedFigure, [summonAbility]);
			await actionState.Perform();
		}
	}
}