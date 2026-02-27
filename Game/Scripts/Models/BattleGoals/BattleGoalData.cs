using System;

public class BattleGoalData
{
	public int Progress { get; private set; }

	public event Action<BattleGoalData> ProgressChangedEvent;

	public BattleGoalData()
	{
	}

	public void AdjustProgress(int value)
	{
		Progress += value;

		// if(GameController.Instance == null || !GameController.FastForward)
		// {
		// 	AppController.Instance!.PersonalQuestProgressUpdateView.AddItem(classModel, personalQuestModel, this);
		// }

		ProgressChangedEvent?.Invoke(this);
	}
}