using Fractural.Tasks;

public class MinorPowerPotion : Prosperity1Item
{
	public override string Name => "Minor Power Potion";
	public override int ItemNumber => 14;
	public override int ShopCount => 2;
	public override int Cost => 10;
	public override ItemType ItemType => ItemType.Small;
	public override ItemUseType ItemUseType => ItemUseType.Consume;

	protected override int AtlasIndex => 30;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringAttack(
			canApply: state => state.Performer == Owner,
			apply: async state =>
			{
				await Use(async user =>
				{
					state.AbilityAdjustAttackValue(1);

					await GDTask.CompletedTask;
				});
			}
		);
	}
}