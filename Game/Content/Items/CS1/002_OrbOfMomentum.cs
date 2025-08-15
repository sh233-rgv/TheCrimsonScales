using Fractural.Tasks;

public class OrbOfMomentum : CS1Item
{
	public override string Name => "Orb of Momentum";
	public override int ItemNumber => 2;
	public override int ShopCount => 2;
	public override int Cost => 15;
	public override ItemType ItemType => ItemType.Small;
	public override ItemUseType ItemUseType => ItemUseType.Consume;
	public override int MaxUseCount => 3;

	protected override int AtlasIndex => 2;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringMove(
			canApply: state => state.Performer == Owner,
			apply: async state =>
			{
				await Use(async user =>
				{
					state.AdjustMoveValue(1);

					await GDTask.CompletedTask;
				});
			},
			canApplyMultipleTimesDuringAbility: false
		);
	}
}