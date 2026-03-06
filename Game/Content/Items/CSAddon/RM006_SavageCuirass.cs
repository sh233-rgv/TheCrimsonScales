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


		ScenarioEvents.LosingCardToNegateDamageEvent.Subscribe(this, _subscriber,
			parameters => parameters.AbilityCard.CardState == CardState.Hand && parameters.Character == Owner,
			async parameters =>
			{
				await Use(async user =>
				{
					parameters.SetResultingCardState(CardState.Discarded);
					await AbilityCmd.AddCondition(null, user, Conditions.Wound1);
				});
			}, order: -10);
	}

	protected override void Unsubscribe()
	{
		base.Unsubscribe();

		ScenarioEvents.LosingCardToNegateDamageEvent.Unsubscribe(this, _subscriber);
	}
}