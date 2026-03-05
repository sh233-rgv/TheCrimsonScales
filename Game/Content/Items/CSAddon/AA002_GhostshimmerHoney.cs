using System.Collections.Generic;
using Fractural.Tasks;
using Godot;

public class GhostshimmerHoney : CSAddonAA
{
	public override string Name => "Ghostshimmer Honey";
	public override int ItemNumber => 2;
	public override int ShopCount => 1;
	public override int Cost => 35;
	public override ItemType ItemType => ItemType.Small;
	public override ItemUseType ItemUseType => ItemUseType.Consume;

	protected override int AtlasIndex => 1;

	protected override void Subscribe()
	{
		base.Subscribe();

		SubscribeDuringTurn(
			canApply: character => character == Owner,
			apply: async character =>
			{
				await Use(async user =>
				{
					ActionState actionState = new ActionState(user,
					[
						HealAbility.Builder()
							.WithHealValue(3)
							.WithTargets(2)
							.WithTarget(Target.SelfOrAllies | Target.SelfCountsForTargets)
							.WithConditions(Conditions.Regenerate)
					]);
					await actionState.Perform();
				});
			}
		);
	}
}