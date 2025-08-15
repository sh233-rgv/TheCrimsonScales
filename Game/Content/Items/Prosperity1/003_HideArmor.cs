using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class HideArmor : Prosperity1Item
{
	public override string Name => "Hide Armor";
	public override int ItemNumber => 3;
	public override int ShopCount => 2;
	public override int Cost => 10;
	public override ItemType ItemType => ItemType.Body;
	public override ItemUseType ItemUseType => ItemUseType.Spend;
	public override int MinusOneCount => 2;

	protected override int AtlasIndex => 4;

	protected override List<ItemUseSlot> GetUseSlots() =>
	[
		new ItemUseSlot(new Vector2(0.22900093f, 0.79751134f)),
		new ItemUseSlot(new Vector2(0.53449905f, 0.79751134f))
	];

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeSufferDamage(
			canApply: parameters => parameters.FromAttack && parameters.Figure == Owner && parameters.WouldSufferDamage,
			apply: async parameters =>
			{
				await Use(async user =>
				{
					parameters.AdjustShield(1);

					await GDTask.CompletedTask;
				});
			}
		);
	}
}