using System;
using Fractural.Tasks;
using Godot;

public class AMDCard : IDeckCard
{
	public AMDCardModel Model { get; }
	public AMDCardOwner Owner { get; }
	public Figure PotentialOriginalOwner { get; }

	public bool Reshuffles => Model.Reshuffles;
	public bool RemoveAfterDraw => Model.RemoveAfterDraw;

	public event Action<AMDCard> DrawnEvent;

	public AMDCard(AMDCardModel model, AMDCardOwner owner, Figure potentialOriginalOwner = null)
	{
		Model = model;
		Owner = owner;
		PotentialOriginalOwner = potentialOriginalOwner;
	}

	public async GDTask<AMDCardValue> Draw(AttackAbility.State attackAbilityState)
	{
		ScenarioEvents.AMDCardDrawn.Parameters amdCardDrawnParameters =
			await ScenarioEvents.AMDCardDrawnEvent.CreatePrompt(
				new ScenarioEvents.AMDCardDrawn.Parameters(attackAbilityState, this));

		AMDCardModel model = amdCardDrawnParameters.OverrideAMDCardModel ?? Model;
		return new AMDCardValue(model.GetRolling(attackAbilityState), amdCardDrawnParameters.Type, amdCardDrawnParameters.Value, model.Pierce,
			model.Push, model.Pull, model.Swing, model.AddedTargets, model.ElementInfusions, model.GetConditionModels(attackAbilityState),
			model.GetAbilities(attackAbilityState), model.GetExtraEffects(attackAbilityState));
	}

	public Texture2D GetTexture()
	{
		return Model.GetTexture(Owner);
	}

	public virtual void Drawn()
	{
		DrawnEvent?.Invoke(this);
	}
}