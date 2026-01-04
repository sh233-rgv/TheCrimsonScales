using Fractural.Tasks;
using Godot;
using Newtonsoft.Json;

public abstract class PersonalQuestModel<T> : PersonalQuestModel
	where T : PersonalQuestData, new()
{
	public sealed override PersonalQuestData CreateData()
	{
		return new T();
	}

	public override bool GetCanRetire(PersonalQuestData personalQuestData)
	{
		return GetCanRetire((T)personalQuestData);
	}

	public sealed override async GDTask OnScenarioSetupPhaseCompleted(Character character)
	{
		// Clone the quest data to overwrite the original later, after the scenario is finished
		T personalQuestData = (T)character.SavedCharacter.SavedPersonalQuest.PersonalQuestData;
		string serializedData = JsonConvert.SerializeObject(personalQuestData, SaveFile.JsonSerializerSettings);
		T clonedQuestData = JsonConvert.DeserializeObject<T>(serializedData);

		await OnScenarioSetupPhaseCompleted(character, (T)clonedQuestData);

		GameController.Instance.EndEvent += OnEndEvent;
		return;

		void OnEndEvent(ScenarioResult scenarioResult, SavedScenarioProgress savedScenarioProgress)
		{
			character.SavedCharacter.SavedPersonalQuest.OverwritePersonalQuestData(personalQuestData);
		}
	}

	protected virtual bool GetCanRetire(T personalQuestData)
	{
		return personalQuestData.Progress >= MaxProgress;
	}

	protected virtual async GDTask OnScenarioSetupPhaseCompleted(Character character, T personalQuestData)
	{
		await GDTask.CompletedTask;
	}
}

public abstract class PersonalQuestModel : AbstractModel
{
	public abstract string Name { get; }
	public abstract ClassModel ClassToUnlock { get; }
	public abstract int MaxProgress { get; }
	public virtual ScenarioModel UnlockedScenarioModel => null;

	protected abstract string TexturePath { get; }
	protected abstract int ColumnCount { get; }
	protected abstract int RowCount { get; }
	protected abstract int AtlasIndex { get; }

	public Texture2D GetTexture()
	{
		return AtlasTextureHelper.CreateAtlasTexture(
			AtlasIndex, ColumnCount, RowCount,
			ResourceLoader.Load<Texture2D>(TexturePath));
	}

	public abstract PersonalQuestData CreateData();

	public abstract bool GetCanRetire(PersonalQuestData personalQuestData);

	public abstract GDTask OnScenarioSetupPhaseCompleted(Character character);
}