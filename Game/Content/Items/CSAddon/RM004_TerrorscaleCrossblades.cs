using System.Collections.Generic;
using System.Linq;
using Fractural.Tasks;
using Godot;

public class TerrorscaleCrossblades : CSAddonRM
{
	public override string Name => "Terrorscale Crossblades";
	public override int ItemNumber => 4;
	public override int ShopCount => 1;
	public override int Cost => 40;
	public override ItemType ItemType => ItemType.TwoHands;
	public override ItemUseType ItemUseType => ItemUseType.Spend;

	protected override int AtlasIndex => 15;

	protected override List<ItemUseSlot> GetUseSlots() =>
	[
		new ItemUseSlot(new Vector2(0.21488501f, 0.79682535f)),
		new ItemUseSlot(new Vector2(0.48124436f, 0.79682535f)),
		new ItemUseSlot(new Vector2(0.7476037f, 0.79682535f))
	];

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeAttackAfterTargetConfirmed(
			canApply: state => state.Performer == Owner && state.Target.Conditions.Any(condition => condition.ConditionModel.IsNegative),
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