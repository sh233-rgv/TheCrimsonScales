using Fractural.Tasks;

public abstract class BattleGoalModel : AbstractModel
{
	public abstract string Title { get; }
	public abstract string Description { get; }

	public virtual BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.One;

	public virtual int MaxProgress => 1;
	public virtual bool FailIfProgressFull => true;

	public async GDTask OnScenarioSetupPhaseCompleted(Character character)
	{
		await OnScenarioSetupPhaseCompleted(character, new BattleGoalData(this));
	}

	protected abstract GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoalData battleGoalData);
}