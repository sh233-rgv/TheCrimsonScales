using Fractural.Tasks;
using Godot;

public abstract class PersonalQuestModel<T> : PersonalQuestModel
	where T : PersonalQuestData, new()
{
	public sealed override PersonalQuestData CreateData()
	{
		return new T();
	}

	public sealed override async GDTask OnScenarioSetupPhaseCompleted(Figure figure, SavedPersonalQuest savedPersonalQuest)
	{
		await base.OnScenarioSetupPhaseCompleted(figure, savedPersonalQuest);

		await OnScenarioSetupPhaseCompleted(figure, (T)savedPersonalQuest.PersonalQuestData);
	}

	public virtual async GDTask OnScenarioSetupPhaseCompleted(Figure figure, T personalQuestData)
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

	public abstract PersonalQuestData CreateData();

	public Texture2D GetTexture()
	{
		return AtlasTextureHelper.CreateAtlasTexture(
			AtlasIndex, ColumnCount, RowCount,
			ResourceLoader.Load<Texture2D>(TexturePath));
	}

	public virtual async GDTask OnScenarioSetupPhaseCompleted(Figure figure, SavedPersonalQuest savedPersonalQuest)
	{
		await GDTask.CompletedTask;
	}
}