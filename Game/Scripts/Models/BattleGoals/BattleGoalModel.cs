using Fractural.Tasks;

public abstract class BattleGoalModel : AbstractModel
{
	public abstract string Title { get; }
	public abstract string Description { get; }

	public virtual BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.One;

	public virtual int MaxProgress => 1;
	public virtual bool FailIfProgressFull => true;

	public abstract GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoalData battleGoalData);
}