using System;

public class PersonalQuestData
{
	public int Progress { get; private set; }

	public event Action<PersonalQuestData> ProgressChangedEvent;

	public void AdjustProgress(int value, Character character)
	{
		AdjustProgress(value, character.SavedCharacter.ClassModel, character.SavedCharacter.SavedPersonalQuest.Model);
	}

	public void AdjustProgress(int value, ClassModel classModel, PersonalQuestModel personalQuestModel)
	{
		Progress += value;

		if(GameController.Instance == null || !GameController.FastForward)
		{
			AppController.Instance!.PersonalQuestProgressUpdateView.AddItem(classModel, personalQuestModel, this);
		}

		ProgressChangedEvent?.Invoke(this);
	}
}