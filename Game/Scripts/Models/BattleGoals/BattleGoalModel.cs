using Fractural.Tasks;

public abstract class BattleGoalModel : AbstractModel, IEventSubscriber
{
	public abstract string Title { get; }
	public abstract string Description { get; }

	public virtual BattleGoalCheckmarkCount CheckmarkCount => BattleGoalCheckmarkCount.One;

	public virtual int MaxProgress => 1;
	public virtual bool FailIfProgressFull => false;

	protected int CharacterCount => GameController.Instance?.CharacterManager.Characters.Count ?? 2;

	public abstract GDTask OnScenarioSetupPhaseCompleted(Character character, BattleGoal battleGoalData);
}