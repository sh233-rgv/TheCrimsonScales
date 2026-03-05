using System.Collections.Generic;
using Godot;

public class SavageCuirass : CSAddonRM
{
	public override string Name => "Savage Cuirass";
	public override int ItemNumber => 6;
	public override int ShopCount => 1;
	public override int Cost => 50;
	public override ItemType ItemType => ItemType.Body;
	public override ItemUseType ItemUseType => ItemUseType.Consume;

	protected override int AtlasIndex => 17;

	private object _subscriber;

	public override void Init(Character owner)
	{
		_subscriber = new object();

		base.Init(owner);
	}

	protected override List<ItemUseSlot> GetUseSlots() =>
	[
		new ItemUseSlot(new Vector2(0.34071124f, 0.821164f)),
		new ItemUseSlot(new Vector2(0.6209605f, 0.821164f))
	];

	protected override void Subscribe()
	{
		base.Subscribe();

		ScenarioEvents.BeforeAbilityCardStateChangedEvent.Subscribe(this, _subscriber,
			parameters => parameters.AbilityCard.Owner == Owner && parameters.FromSufferDamage && parameters.AbilityCard.CardState is CardState.Hand,
			async parameters =>
			{
				await Use(async user =>
				{
					parameters.SetNewCardState(CardState.Discarded);
					await AbilityCmd.AddCondition(null, user, Conditions.Wound1);
				});
			});
	}

	protected override void Unsubscribe()
	{
		base.Unsubscribe();

		ScenarioEvents.BeforeAbilityCardStateChangedEvent.Unsubscribe(this, _subscriber);
	}
}