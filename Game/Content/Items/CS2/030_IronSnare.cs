using Fractural.Tasks;

public class IronSnare : CS2Item
{
	public override string Name => "Iron Snare";
	public override int ItemNumber => 30;
	public override int ShopCount => 2;
	public override int Cost => 15;
	public override ItemType ItemType => ItemType.Small;
	public override ItemUseType ItemUseType => ItemUseType.Consume;

	protected override int AtlasIndex => 0;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringTurn(
			canApply: character => character == Owner,
			apply: async character =>
			{
				await Use(async user =>
				{
					ActionState actionState = new ActionState(character, [CreateTrapAbility.Builder()
						.WithDamage(0)
						.WithConditions(Conditions.Immobilize)
						.WithRange(3)
						.Build()]);
					await actionState.Perform();
				});
			}
		);
	}
}