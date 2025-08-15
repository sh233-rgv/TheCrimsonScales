using Fractural.Tasks;

public class OrbOfInfection : CS1Item
{
	public override string Name => "Orb of Infection";
	public override int ItemNumber => 6;
	public override int ShopCount => 2;
	public override int Cost => 20;
	public override ItemType ItemType => ItemType.Small;
	public override ItemUseType ItemUseType => ItemUseType.Consume;
	public override int MaxUseCount => 3;

	protected override int AtlasIndex => 10;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringAttack(
			canApply: state => state.Performer == Owner,
			apply: async state =>
			{
				await Use(async user =>
				{
					state.SingleTargetAddCondition(Conditions.Poison1);

					await GDTask.CompletedTask;
				});
			}
		);
	}
}