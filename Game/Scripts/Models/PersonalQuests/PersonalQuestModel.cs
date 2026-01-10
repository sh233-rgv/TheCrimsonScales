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

	public override bool GetCanRetire(SavedCampaign savedCampaign, PersonalQuestData personalQuestData)
	{
		return GetCanRetire(savedCampaign, (T)personalQuestData);
	}

	public sealed override async GDTask OnScenarioSetupPhaseCompleted(Character character)
	{
		// Clone the quest data to overwrite the original later, after the scenario is finished
		T personalQuestData = (T)character.SavedCharacter.SavedPersonalQuest.PersonalQuestData;
		string serializedData = JsonConvert.SerializeObject(personalQuestData, SaveFile.JsonSerializerSettings);
		T clonedQuestData = JsonConvert.DeserializeObject<T>(serializedData);

		await OnScenarioSetupPhaseCompleted(character, clonedQuestData);

		GameController.Instance.EndEvent += OnEndEvent;
		return;

		void OnEndEvent(ScenarioResult scenarioResult, SavedScenarioProgress savedScenarioProgress)
		{
			character.SavedCharacter.SavedPersonalQuest.OverwritePersonalQuestData(clonedQuestData);
		}
	}

	protected virtual bool GetCanRetire(SavedCampaign savedCampaign, T personalQuestData)
	{
		return RequiredCompletedScenario == null
			? personalQuestData.Progress >= MaxProgress
			: savedCampaign.SavedScenarioProgresses.GetScenarioProgress(RequiredCompletedScenario).Completed;
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
	public virtual ScenarioModel RequiredCompletedScenario => null;

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

	public abstract bool GetCanRetire(SavedCampaign savedCampaign, PersonalQuestData personalQuestData);

	public virtual async GDTask OnBetweenScenariosStarted(SavedCharacter savedCharacter)
	{
		if(UnlockedScenarioModel != null && savedCharacter.SavedPersonalQuest.PersonalQuestData.Progress >= MaxProgress)
		{
			BetweenScenariosController.Instance.SavedCampaign.SavedScenarioProgresses.GetScenarioProgress(UnlockedScenarioModel).Discover();
		}

		await GDTask.CompletedTask;
	}

	public abstract GDTask OnScenarioSetupPhaseCompleted(Character character);
}