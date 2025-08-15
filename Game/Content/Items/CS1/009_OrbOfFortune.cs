using Fractural.Tasks;

public class OrbOfFortune : CS1Item
{
	public override string Name => "Orb of Fortune";
	public override int ItemNumber => 9;
	public override int ShopCount => 2;
	public override int Cost => 30;
	public override ItemType ItemType => ItemType.Small;
	public override ItemUseType ItemUseType => ItemUseType.Consume;
	public override int MaxUseCount => 3;

	protected override int AtlasIndex => 16;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringAttack(
			canApply: state => state.Performer == Owner,
			apply: async state =>
			{
				await Use(async user =>
				{
					state.SingleTargetSetHasAdvantage();

					await GDTask.CompletedTask;
				});
			}
		);
	}
}