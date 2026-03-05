using Fractural.Tasks;

public class TerrorscaleSaber : CSAddonRM
{
	public override string Name => "Terrorscale Saber";
	public override int ItemNumber => 3;
	public override int ShopCount => 1;
	public override int Cost => 40;
	public override ItemType ItemType => ItemType.OneHand;
	public override ItemUseType ItemUseType => ItemUseType.Spend;

	protected override int AtlasIndex => 14;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeAttackAfterTargetConfirmed(
			canApply: state => state.Performer == Owner && state.SingleTargetRangeType == RangeType.Melee && state.Target.HasWound() ||
			                   state.Target.HasCondition(Conditions.Rupture),
			apply: async state =>
			{
				await Use(async user =>
				{
					state.SingleTargetAdjustAttackValue(1);
					state.SingleTargetAdjustPierce(2);

					await GDTask.CompletedTask;
				});
			}
		);
	}
}