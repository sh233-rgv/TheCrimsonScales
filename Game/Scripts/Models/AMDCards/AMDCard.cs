using System;
using Fractural.Tasks;
using Godot;

public class AMDCard : IDeckCard
{
	public AMDCardModel Model { get; }
	public AMDCardOwner Owner { get; }

	public bool Reshuffles => Model.Reshuffles;
	public bool RemoveAfterDraw => Model.RemoveAfterDraw;

	public event Action<AMDCard> DrawnEvent;

	public AMDCard(AMDCardModel model, AMDCardOwner owner)
	{
		Model = model;
		Owner = owner;
	}

	public async GDTask<AMDCardValue> Draw(AttackAbility.State attackAbilityState)
	{
		ScenarioEvents.AMDCardDrawn.Parameters amdCardDrawnParameters =
			await ScenarioEvents.AMDCardDrawnEvent.CreatePrompt(
				new ScenarioEvents.AMDCardDrawn.Parameters(attackAbilityState, this));

		return new(amdCardDrawnParameters.Type, amdCardDrawnParameters.Value);
	}

	public Texture2D GetTexture()
	{
		return Model.GetTexture();
	}

	public virtual void Drawn()
	{
		DrawnEvent?.Invoke(this);
	}
}