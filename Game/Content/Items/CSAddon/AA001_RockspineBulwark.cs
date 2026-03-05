using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class RockspineBulwark : CSAddonAA
{
	public override string Name => "Rockspine Bulwark";
	public override int ItemNumber => 1;
	public override int ShopCount => 1;
	public override int Cost => 35;
	public override ItemType ItemType => ItemType.Body;
	public override ItemUseType ItemUseType => ItemUseType.Spend;
	public override int MinusOneCount => 3;

	protected override int AtlasIndex => 0;

	protected override List<ItemUseSlot> GetUseSlots() =>
	[
		new ItemUseSlot(new Vector2(0.1715812f, 0.7867725f)),
		new ItemUseSlot(new Vector2(0.48287848f, 0.7867725f)),
		new ItemUseSlot(new Vector2(0.79417574f, 0.7867725f))
	];

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeSufferDamage(
			canApply: parameters => parameters.FromAttack && parameters.Figure.AlliedWith(Owner, true) &&
			                        RangeHelper.Distance(parameters.Figure.Hex, Owner.Hex) <= 1 && parameters.WouldSufferDamage,
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