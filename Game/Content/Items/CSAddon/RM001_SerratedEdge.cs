using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class SerratedEdge : CSAddonRM
{
	public override string Name => "Serrated Edge";
	public override int ItemNumber => 1;
	public override int ShopCount => 2;
	public override int Cost => 25;
	public override ItemType ItemType => ItemType.OneHand;
	public override ItemUseType ItemUseType => ItemUseType.Spend;

	protected override int AtlasIndex => 11;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringAttack(
			canApply: state => state.Performer == Owner && state.SingleTargetRangeType == RangeType.Melee,
			apply: async state =>
			{
				await Use(async user =>
				{
					state.SingleTargetAddCondition(Conditions.Rupture);

					await GDTask.CompletedTask;
				});
			}
		);
	}
}