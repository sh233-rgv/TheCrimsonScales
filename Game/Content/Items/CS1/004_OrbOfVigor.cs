using Fractural.Tasks;

public class OrbOfVigor : CS1Item
{
	public override string Name => "Orb of Vigor";
	public override int ItemNumber => 4;
	public override int ShopCount => 2;
	public override int Cost => 20;
	public override ItemType ItemType => ItemType.Small;
	public override ItemUseType ItemUseType => ItemUseType.Consume;
	public override int MaxUseCount => 3;

	protected override int AtlasIndex => 6;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringAttack(
			canApply: state => state.Performer == Owner,
			apply: async state =>
			{
				await Use(async user =>
				{
					state.SingleTargetAdjustAttackValue(1);

					await GDTask.CompletedTask;
				});
			},
			canApplyMultipleTimesDuringAbility: false
		);
	}
}