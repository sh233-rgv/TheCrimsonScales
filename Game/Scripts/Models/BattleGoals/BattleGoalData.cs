using System;

public class BattleGoalData
{
	public int Progress { get; private set; }

	public event Action<BattleGoalData> ProgressChangedEvent;

	private void AdjustProgress(int value, BattleGoalModel battleGoalModel)
	{
		Progress += value;

		// if(GameController.Instance == null || !GameController.FastForward)
		// {
		// 	AppController.Instance!.PersonalQuestProgressUpdateView.AddItem(classModel, personalQuestModel, this);
		// }

		ProgressChangedEvent?.Invoke(this);
	}
}